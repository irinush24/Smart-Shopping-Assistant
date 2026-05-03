using SmartShoppingAssistantLigaAc.DataAccess.Entities;

namespace SmartShoppingAssistantLigaAc.DataAccess.Repositories.Interfaces;

public interface ICartItemRepository : IRepository<CartItem>
{
    Task<List<CartItem>> GetAllWithProductsAsync();

    Task<CartItem> GetByIdWithProductsAsync(int id);
}
