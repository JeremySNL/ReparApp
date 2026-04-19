using Microsoft.EntityFrameworkCore;
using ReparApp.Data;
using ReparApp.Models;
using System.Linq.Expressions;

public class RepairStatusService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Guardar(RepairStatus entidad)
    {
        if (!await Existe(entidad.RepairStatusId))
            return await Insertar(entidad);
        else
            return await Modificar(entidad);
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.RepairStatuses.AnyAsync(s => s.RepairStatusId == id);
    }

    private async Task<bool> Insertar(RepairStatus entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.RepairStatuses.Add(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(RepairStatus entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.RepairStatuses.Update(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<RepairStatus?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.RepairStatuses.AsNoTracking().FirstOrDefaultAsync(s => s.RepairStatusId == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var usado = await contexto.Repairs.AnyAsync(r => r.RepairStatusId == id);
        if (usado)
            throw new InvalidOperationException("El estado está en uso");

        return await contexto.RepairStatuses.Where(s => s.RepairStatusId == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<RepairStatus>> GetList(Expression<Func<RepairStatus, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.RepairStatuses.Where(criterio).AsNoTracking().ToListAsync();
    }
}