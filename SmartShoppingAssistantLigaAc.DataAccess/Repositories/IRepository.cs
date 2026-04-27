using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistantLigaAc.DataAccess.Repositories;

// CRUD operations

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
    Task<List<TEntity>> GetAllAsync();

    Task <TEntity> AddAsync(TEntity entity);

    Task <TEntity> UpdateAsync(TEntity entity);

    Task DeleteAsync(int id);

}
