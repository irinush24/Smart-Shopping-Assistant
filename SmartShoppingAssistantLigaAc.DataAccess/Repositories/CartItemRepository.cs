using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using SmartShoppingAssistantLigaAc.DataAccess.Repositories.Interfaces;

namespace SmartShoppingAssistantLigaAc.DataAccess.Repositories;

public class CartItemRepository: BaseRepository<CartItem>, ICartItemRepository
{
    private readonly SmartShoppingAssistantDbContext context;

    public CartItemRepository(SmartShoppingAssistantDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<List<CartItem>> GetAllWithProductsAsync()
    {
        return await context.CartItems.Include(ci => ci.Product).ToListAsync();
    }

    public async Task<CartItem> GetByIdWithProductsAsync(int id)
    {
        return await context.CartItems.Include(ci => ci.Product).FirstOrDefaultAsync(ci => ci.Id == id);
    }
}
