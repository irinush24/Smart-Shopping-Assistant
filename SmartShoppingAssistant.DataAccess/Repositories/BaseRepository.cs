using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories.Interfaces;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public class BaseRepository<TEntity>(SmartShoppingAssistantDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    public IQueryable<TEntity> GetAllAsQueryable() => context.Set<TEntity>().AsQueryable();

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await context.Set<TEntity>().FindAsync(id)
            ?? throw new Exception($"Entity with id {id} not found");
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.Set<TEntity>().FindAsync(id)
            ?? throw new Exception($"Entity with id {id} not found");

        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync();
    }
}