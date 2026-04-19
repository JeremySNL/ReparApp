using Microsoft.EntityFrameworkCore;
using ReparApp.Data;
using ReparApp.Models;
using System.Linq.Expressions;

public class RepairService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Guardar(Repair entidad)
    {
        if (!await Existe(entidad.RepairId))
            return await Insertar(entidad);
        else
            return await Modificar(entidad);
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Repairs.AnyAsync(r => r.RepairId == id);
    }

    private async Task<bool> Insertar(Repair entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Repairs.Add(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Repair entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Repairs.Update(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Repair?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Repairs
            .Include(r => r.Customer).ThenInclude(c => c.Person)
            .Include(r => r.Technician).ThenInclude(t => t.Person)
            .Include(r => r.RepairStatus)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RepairId == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Repairs.Where(r => r.RepairId == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Repair>> GetList(Expression<Func<Repair, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Repairs
            .Include(r => r.Customer)
            .Include(r => r.Technician)
            .Include(r => r.RepairStatus)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}