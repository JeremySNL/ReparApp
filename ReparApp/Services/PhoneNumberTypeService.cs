using Microsoft.EntityFrameworkCore;
using ReparApp.Models;
using ReparApp.Data;
using System.Linq.Expressions;

namespace ReparApp.Services;

public class PhoneNumberTypeService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Guardar(PhoneNumberType entidad)
    {
        if (!await Existe(entidad.PhoneNumberTypeId))
        {
            return await Insertar(entidad);
        }
        else
        {
            return await Modificar(entidad);
        }
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.PhoneNumberTypes.AnyAsync(p => p.PhoneNumberTypeId == id);
    }

    private async Task<bool> Insertar(PhoneNumberType entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.PhoneNumberTypes.Add(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(PhoneNumberType entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.PhoneNumberTypes.Update(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<PhoneNumberType?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.PhoneNumberTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PhoneNumberTypeId == id);
    }

    public async Task<bool> BuscarDuplicado(string nombre, int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.PhoneNumberTypes.AnyAsync(p =>
            p.Name.ToLower().Equals(nombre.Trim().ToLower()) &&
            p.PhoneNumberTypeId != id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var existe = await contexto.PhoneNumberTypes.AnyAsync(p => p.PhoneNumberTypeId == id);
        if (!existe)
        {
            throw new InvalidOperationException("No se puede eliminar: el tipo de número no existe");
        }

        var tieneTelefonos = await contexto.PhoneNumbers.AnyAsync(p => p.PhoneNumberTypeId == id);
        if (tieneTelefonos)
        {
            throw new InvalidOperationException("No se puede eliminar: el tipo de número está en uso");
        }

        return await contexto.PhoneNumberTypes
            .Where(p => p.PhoneNumberTypeId == id)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<List<PhoneNumberType>> GetList(Expression<Func<PhoneNumberType, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.PhoneNumberTypes
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}