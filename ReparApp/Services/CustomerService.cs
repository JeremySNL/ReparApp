using Microsoft.EntityFrameworkCore;
using ReparApp.Data;
using ReparApp.Models;
using System.Linq.Expressions;

public class CustomerService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Guardar(Customer entidad)
    {
        if (!await Existe(entidad.PersonId))
            return await Insertar(entidad);
        else
            return await Modificar(entidad);
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Customers.AnyAsync(c => c.PersonId == id);
    }

    private async Task<bool> Insertar(Customer entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Customers.Add(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Customer entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Customers.Update(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Customer?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Customers
            .Include(c => c.Person)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.PersonId == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var tieneReparaciones = await contexto.Repairs.AnyAsync(r => r.CustomerId == id);
        if (tieneReparaciones)
            throw new InvalidOperationException("El cliente tiene reparaciones asociadas");

        return await contexto.Customers.Where(c => c.PersonId == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Customer>> GetList(Expression<Func<Customer, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Customers
            .Include(c => c.Person)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}