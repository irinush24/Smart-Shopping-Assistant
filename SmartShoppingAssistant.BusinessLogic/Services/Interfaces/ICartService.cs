using SmartShoppingAssistant.BusinessLogic.DTOs;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

public interface ICartService
{
    Task<CartGetDTO> GetCartAsync();
    Task<CartGetDTO> AddItemAsync(AddCartItemDTO dto);
    Task<CartGetDTO> UpdateItemAsync(int itemId, UpdateCartItemDTO dto);
    Task<CartGetDTO> RemoveItemAsync(int itemId);
    Task ClearCartAsync();
}