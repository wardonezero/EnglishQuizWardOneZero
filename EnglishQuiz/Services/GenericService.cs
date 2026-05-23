using EnglishQuiz.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishQuiz.Services;

public sealed class GenericService<TContext>(TContext context) where TContext : DbContext
{
    public async Task<List<TEntity>> GetPagedAsync<TEntity>(int page = 1, byte size = 20) where TEntity : class
    {
        return await context.Set<TEntity>().AsNoTracking()
            .OrderBy(e => EF.Property<int>(e, "Id"))
            .Skip((page - 1) * size).Take(size)
            .ToListAsync();
    }

    public async Task<List<RIdName>> GetItemIdNameOnlyPagedAsync<TEntity>(int page = 1, byte size = 100) where TEntity : class
    {
        List<RIdName> items = await context.Set<TEntity>().AsNoTracking()
            .Select(e => new RIdName
            {
                Id = EF.Property<int>(e, "Id"),
                Name = EF.Property<string>(e, "Name") ?? string.Empty
            })
            .OrderBy(e => e.Id)
            .Skip((page - 1) * size).Take(size).ToListAsync();
        return items;
    }

    public async Task<TEntity?> GetAsync<TEntity>(int id) where TEntity : class
    {
        return await context.Set<TEntity>().AsNoTracking()
            .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<bool> DeleteManyAsync<TEntity>(List<int> ids) where TEntity : class
    {
        if (ids is null || ids.Count == 0) return false;
        try
        {
            int affectedRows = await context.Set<TEntity>()
                .Where(e => ids.Contains(EF.Property<int>(e, "Id")))
                .ExecuteDeleteAsync();
            return affectedRows == ids.Count;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync<TEntity>(int id) where TEntity : class
    {
        try
        {
            int affectedRows = await context.Set<TEntity>()
                .Where(e => EF.Property<int>(e, "Id") == id)
                .ExecuteDeleteAsync();
            return affectedRows > 0 && affectedRows < 2;
        }
        catch
        {
            //TODO: Add logging
            return false;
        }
    }

    public async Task<TEntity?> EditAsync<TEntity>(TEntity entity) where TEntity : class
    {
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync();
        context.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    public async Task<int> CreateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        context.Set<TEntity>().Add(entity);
        await context.SaveChangesAsync();
        int id = (int)context.Entry(entity).Property("Id").CurrentValue!;
        context.Entry(entity).State = EntityState.Detached;
        return id;
    }
}