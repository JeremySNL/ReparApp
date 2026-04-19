using Microsoft.EntityFrameworkCore;
using ReparApp.Data;
using ReparApp.Models;
using System.Linq.Expressions;

namespace ReparApp.Services;

public class PersonService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Guardar(Person entidad)
    {
        if (!await Existe(entidad.PersonId))
            return await Insertar(entidad);
        else
            return await Modificar(entidad);
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Persons.AnyAsync(p => p.PersonId == id);
    }

    private async Task<bool> Insertar(Person entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Persons.Add(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Person entidad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Persons.Update(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Person?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Persons.AsNoTracking().FirstOrDefaultAsync(p => p.PersonId == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var existe = await contexto.Persons.AnyAsync(p => p.PersonId == id);
        if (!existe)
            throw new InvalidOperationException("La persona no existe");

        return await contexto.Persons.Where(p => p.PersonId == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Person>> GetList(Expression<Func<Person, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Persons.Where(criterio).AsNoTracking().ToListAsync();
    }
}