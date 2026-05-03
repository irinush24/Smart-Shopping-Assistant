using System;
using System.Collections.Generic;
using System.Text;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;

namespace SmartShoppingAssistantLigaAc.DataAccess.Repositories;

public class ProductRepository(SmartShoppingAssistantDbContext context) : IProductRepository<Product>
{

}
