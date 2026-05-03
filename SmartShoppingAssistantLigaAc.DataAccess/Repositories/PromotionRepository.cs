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
        return await context.Promotions.Include(p => p.Products).Include(p => p.Categories).Where(p => p.IsActive == true).ToListAsync();
    }
}
