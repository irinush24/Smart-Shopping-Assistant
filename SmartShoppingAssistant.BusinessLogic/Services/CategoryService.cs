using SmartShoppingAssistantLigaAc.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class CategoryService(IRepository<Category> CategoryRepository) : ICategoryService
{
    public async Task<CategoryGetDTO> GetByIdAsync(int id)
    {
        var Category = await CategoryRepository.GetByIdAsync(id);

        return new CategoryGetDTO
        {
            Id = Category.Id,
            Name = Category.Name,
            Description = Category.Description
        };
    }

    public async Task<CategoryGetDTO> AddAsync(CategoryGetDTO CategoryDTO)
    {
        var Category = new Category
        {
            Name = CategoryDTO.Name,
            Description = CategoryDTO.Description
        };
        var addedCategory = await CategoryRepository.AddAsync(Category);
        return new CategoryGetDTO
        {
            Id = addedCategory.Id,
            Name = addedCategory.Name,
            Description = addedCategory.Description
        };
    }

    public async Task DeleteAsync(int id)
    {
        await CategoryRepository.DeleteAsync(id);
    }

    public async Task<List<CategoryGetDTO>> GetAllAsync()
    {
        var Categorys = await CategoryRepository.GetAllAsync();

        var CategoryDTOs = new List<CategoryGetDTO>();

        foreach (var Category in Categorys)
        {
            CategoryDTOs.Add(new CategoryGetDTO
            {
                Id = Category.Id,
                Name = Category.Name,
                Description = Category.Description
            });
        }

        return CategoryDTOs;
    }

    public async Task<CategoryGetDTO> UpdateAsync(int id, CategoryGetDTO CategoryDTO)
    {
        var existingCategory = await CategoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
        {
            throw new Exception($"Category with id {id} not found");
        }
        existingCategory.Name = CategoryDTO.Name;
        existingCategory.Description = CategoryDTO.Description;
        var updatedCategory = await CategoryRepository.UpdateAsync(existingCategory);
        return new CategoryGetDTO
        {
            Id = updatedCategory.Id,
            Name = updatedCategory.Name,
            Description = updatedCategory.Description
        };
    }
}
