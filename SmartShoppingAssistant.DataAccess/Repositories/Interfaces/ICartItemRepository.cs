using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories.Interfaces;

public interface ICartItemRepository : IRepository<CartItem>
{
    Task<List<CartItem>> GetAllWithProductAsync();
    Task<CartItem?> GetByProductIdAsync(int productId);
    Task<CartItem> GetByIdWithProductAsync(int id);
    Task ClearAsync();
}