using Microsoft.EntityFrameworkCore;
using ReparApp.Data;
using ReparApp.Models;
using System.Linq.Expressions;

public class PhoneNumberService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Guardar(PhoneNumber entidad)
    {
        if (!await Existe(entidad.PhoneNumberId))
            return await Insertar(entidad);
        else
            return await Modificar(entidad);
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.PhoneNumbers.AnyAsync(p => p.PhoneNumberId == id);
    }

    private async Task<bool> Insertar(PhoneNumber entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.PhoneNumbers.Add(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(PhoneNumber entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.PhoneNumbers.Update(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<PhoneNumber?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.PhoneNumbers
            .Include(p => p.Person)
            .Include(p => p.PhoneNumberType)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PhoneNumberId == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.PhoneNumbers.Where(p => p.PhoneNumberId == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<PhoneNumber>> GetList(Expression<Func<PhoneNumber, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.PhoneNumbers
            .Include(p => p.Person)
            .Include(p => p.PhoneNumberType)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}