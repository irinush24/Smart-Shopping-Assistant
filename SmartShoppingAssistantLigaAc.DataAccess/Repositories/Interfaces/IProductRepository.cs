using SmartShoppingAssistantLigaAc.DataAccess.Entities;

namespace SmartShoppingAssistantLigaAc.DataAccess.Repositories.Interfaces;

public interface IProductRepository:IRepository<Product>
{
    Task<Product> GetProductWithCategoriesAsync(int id);

    Task UpdateProductWithCategoriesAsync(int productId, Product updatedProduct, List<int> categoryIdsFromRequest);

    Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
}
