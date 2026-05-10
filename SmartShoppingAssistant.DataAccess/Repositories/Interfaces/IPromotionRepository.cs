using SmartShoppingAssistant.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SmartShoppingAssistant.DataAccess.Repositories.Interfaces;

public interface IPromotionRepository : IRepository<Promotion>
{
    Task<List<Promotion>> GetForProductAsync(int productId);
}
