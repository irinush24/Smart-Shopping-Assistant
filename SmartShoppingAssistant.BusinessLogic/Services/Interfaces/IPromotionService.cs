using SmartShoppingAssistant.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

public interface IPromotionService
{
    Task<PromotionGetDTO> AddAsync(PromotionGetDTO promotion, List<int> categoryID, List<int> productID);

    Task<PromotionGetDTO> GetByIdAsync(int id);

    Task<List<PromotionGetDTO>> GetAllAsync();

    Task<PromotionGetDTO> UpdateAsync(int id, PromotionGetDTO promotionDTO, List<int> newCategoryID, List<int> newProductID);

    Task DeleteAsync(int id);
}
