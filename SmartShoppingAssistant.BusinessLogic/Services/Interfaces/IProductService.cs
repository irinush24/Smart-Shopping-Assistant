using SmartShoppingAssistant.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

public interface IProductService
{
    Task<ProductGetDTO> GetByIdAsync(int id);

    Task DeleteAsync(int id);

    Task<ProductGetDTO> AddAsync(ProductGetDTO product, List<int> categoryIDs);

    Task<List<ProductGetDTO>> GetAllAsync();

    Task<ProductGetDTO> UpdateAsync(int id, ProductGetDTO productDTO, List<int> newCategoryIDs);
}