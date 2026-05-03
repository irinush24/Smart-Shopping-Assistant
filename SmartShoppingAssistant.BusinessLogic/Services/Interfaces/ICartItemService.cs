using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.Requests;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

public interface ICartItemService
{
    Task<CartItemGetDTO> AddAsync(CartItemAddRequest request);
    
    Task<CartGetDTO> GetAsync();
    
    Task<CartItemGetDTO> UpdateAsync(int id, CartItemGetDTO cartItemDTO);
    
    Task DeleteByIdAsync(int id);

    Task DeleteAllAsync();
}
