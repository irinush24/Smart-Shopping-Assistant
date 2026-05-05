using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using SmartShoppingAssistantLigaAc.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SmartShoppingAssistantLigaAc.DataAccess.Repositories;

public class PromotionRepository : BaseRepository<Promotion>, IPromotionRepository
{
    private readonly SmartShoppingAssistantDbContext context;
    public PromotionRepository(SmartShoppingAssistantDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<Promotion> GetPromotionByIdAsync(int id)
    {
        return await context.Promotions.Include(p => p.Categories).Include(p => p.Products).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Promotion>> GetActivePromotionsAsync()
    {
        return await context.Promotions.Include(p => p.Products).ThenInclude(p => p.Categories).Where(p => p.IsActive == true).ToListAsync();
    }

    public async Task<List<Promotion>> GetForProductsAsync(int productId)
    {
        var categoryIds = await context.Products.Where(p=> p.Id == productId).SelectMany(p => p.Categories.Select(c => c.Id)).ToListAsync();

        return await GetAllAsQueryable().Where(p => p.IsActive && (p.Products.Any(p => p.Id == productId)||(p.Categories.Any(c => categoryIds.Contains(c.Id))))).ToListAsync();
    }
}
