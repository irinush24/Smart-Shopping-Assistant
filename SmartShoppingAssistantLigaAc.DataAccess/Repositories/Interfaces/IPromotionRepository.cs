using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SmartShoppingAssistantLigaAc.DataAccess.Repositories.Interfaces;

public interface IPromotionRepository:IRepository<Promotion>
{
    public Task<Promotion> GetPromotionByIdAsync(int id);

    public Task<List<Promotion>> GetActivePromotionsAsync();
}
