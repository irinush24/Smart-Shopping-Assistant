using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using SmartShoppingAssistantLigaAc.DataAccess.Repositories.Interfaces;

namespace SmartShoppingAssistantLigaAc.DataAccess.Repositories;

// Save pentru adaugari

public class BaseRepository<TEntity>(SmartShoppingAssistantDbContext context) : IRepository<TEntity> where TEntity : class
{
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        try
        {
            var addedEntity = context.Set<TEntity>().Add(entity).Entity;
            if (addedEntity == null)
            {
                throw new Exception("Attempted to add null entity");
            }
            context.SaveChanges();
            return addedEntity;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while adding entity: {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var entity = await context.Set<TEntity>().FindAsync(id);

            if(entity == null)
            {
                throw new Exception($"Entity with id {id} not found");
            }
            context.Set<TEntity>().Remove(entity);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while deleting entity with id {id}, {ex.Message}", ex);
        }
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        try
        {
            return await context.Set<TEntity>().ToListAsync();

        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving all entities: {ex.Message}", ex);
        }
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        try
        {
            var entity = await context.Set<TEntity>().FindAsync(id);

            if(entity == null)
            {
                throw new Exception($"Entity with id {id} not found");
            }

            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving entity with id {id}: {ex.Message}", ex);
        }
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        try
        {
            var updatedEntity = context.Set<TEntity>().Update(entity).Entity;
            if (updatedEntity == null)
            {
                throw new Exception("Attempted to update null entity");
            }
            context.SaveChanges();
            return updatedEntity;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while updating entity: {ex.Message}", ex);
        }
    }
}
