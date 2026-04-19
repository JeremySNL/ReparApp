using Microsoft.EntityFrameworkCore;
using ReparApp.Data;
using ReparApp.Models;
using System.Linq.Expressions;

public class RepairStatusHistoryService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Guardar(RepairStatusHistory entidad)
    {
        if (!await Existe(entidad.RepairStatusHistoryId))
            return await Insertar(entidad);
        else
            return await Modificar(entidad);
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.RepairStatusHistories.AnyAsync(h => h.RepairStatusHistoryId == id);
    }

    private async Task<bool> Insertar(RepairStatusHistory entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.RepairStatusHistories.Add(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(RepairStatusHistory entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.RepairStatusHistories.Update(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<RepairStatusHistory?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.RepairStatusHistories
            .Include(h => h.Repair)
            .Include(h => h.RepairStatus)
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.RepairStatusHistoryId == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.RepairStatusHistories.Where(h => h.RepairStatusHistoryId == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<RepairStatusHistory>> GetList(Expression<Func<RepairStatusHistory, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.RepairStatusHistories
            .Include(h => h.Repair)
            .Include(h => h.RepairStatus)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}