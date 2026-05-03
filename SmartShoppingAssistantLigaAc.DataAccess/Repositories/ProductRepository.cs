using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using SmartShoppingAssistantLigaAc.DataAccess.Repositories.Interfaces;

namespace SmartShoppingAssistantLigaAc.DataAccess.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private readonly SmartShoppingAssistantDbContext context;

    public ProductRepository(SmartShoppingAssistantDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<Product> GetProductWithCategoriesAsync(int id)
    {
        return await context.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateProductWithCategoriesAsync(int productId, Product updatedProduct, List<int> categoryIdsFromRequest)
    {
        var existingProduct = await context.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == productId);
        if (existingProduct == null)
        {
            throw new Exception($"Product with ID {productId} not found.");
        }

        // Update product properties
        existingProduct.Name = updatedProduct.Name;
        existingProduct.Description = updatedProduct.Description;
        existingProduct.ImageUrl = updatedProduct.ImageUrl;
        existingProduct.Price = updatedProduct.Price;

        // Update categories
        existingProduct.Categories.Clear();
        foreach (var categoryId in categoryIdsFromRequest)
        {
            var category = await context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                existingProduct.Categories.Add(category);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        return await context.Products.Include(p => p.Categories).Where(p => p.Categories.Any(c => c.Id == categoryId)).ToListAsync();
    }
}
