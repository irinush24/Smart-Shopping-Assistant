using System;
using System.Collections.Generic;
using System.Text;
using SmartShoppingAssistant.BusinessLogic.DTOs;
namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

public interface ICategoryService
{
    Task <CategoryGetDTO> GetByIdAsync(int id);

    Task <List<CategoryGetDTO>> GetAllAsync();

    Task <CategoryGetDTO> AddAsync(CategoryGetDTO category);

    Task DeleteAsync(int id);

    Task <CategoryGetDTO> UpdateAsync(int id, CategoryGetDTO categoryDTO);
}
