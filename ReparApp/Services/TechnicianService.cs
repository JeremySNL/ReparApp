using Microsoft.EntityFrameworkCore;
using ReparApp.Data;
using ReparApp.Models;
using System.Linq.Expressions;

public class TechnicianService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Guardar(Technician entidad)
    {
        if (!await Existe(entidad.PersonId))
            return await Insertar(entidad);
        else
            return await Modificar(entidad);
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Technicians.AnyAsync(t => t.PersonId == id);
    }

    private async Task<bool> Insertar(Technician entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Technicians.Add(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Technician entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Technicians.Update(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Technician?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Technicians.Include(t => t.Person).AsNoTracking().FirstOrDefaultAsync(t => t.PersonId == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var tieneReparaciones = await contexto.Repairs.AnyAsync(r => r.TechnicianId == id);
        if (tieneReparaciones)
            throw new InvalidOperationException("El técnico tiene reparaciones asignadas");

        return await contexto.Technicians.Where(t => t.PersonId == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Technician>> GetList(Expression<Func<Technician, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Technicians.Include(t => t.Person).Where(criterio).AsNoTracking().ToListAsync();
    }
}