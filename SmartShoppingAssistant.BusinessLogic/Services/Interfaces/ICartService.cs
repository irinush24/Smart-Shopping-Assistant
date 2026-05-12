using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Models;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

public interface ICartService
{
    Task<CartGetDTO> GetCartAsync();
    Task<CartGetDTO> AddItemAsync(AddCartItemDTO dto);
    Task<CartGetDTO> UpdateItemAsync(int itemId, UpdateCartItemDTO dto);
    Task<CartGetDTO> RemoveItemAsync(int itemId);
    Task ClearCartAsync();
    Task<PromotionAnalysis> AnalyzeCartAsync();
}