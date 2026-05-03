using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using SmartShoppingAssistantLigaAc.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class ProductService(IRepository<Category> categoryRepository, IProductRepository customProductRepository): IProductService
{
    public async Task<ProductGetDTO> GetByIdAsync(int id)
    {
        var product = await customProductRepository.GetByIdAsync(id);
        return new ProductGetDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            Price = product.Price
        };
    }

    public async Task<ProductGetDTO> AddAsync(ProductGetDTO productDTO, List<int> categoryIDs)
    {
        var product = new Product
        {
            Name = productDTO.Name,
            Description = productDTO.Description,
            ImageUrl = productDTO.ImageUrl,
            Price = productDTO.Price
        };
        foreach(var categoryId in categoryIDs)
        {
            var category = await categoryRepository.GetByIdAsync(categoryId);
            if (category != null)
            {
                product.Categories.Add(category);
            }
        }
        var addedProduct = await customProductRepository.AddAsync(product);
        return new ProductGetDTO
        {
            Id = addedProduct.Id,
            Name = addedProduct.Name,
            Description = addedProduct.Description,
            ImageUrl = addedProduct.ImageUrl,
            Price = addedProduct.Price
        };
    }

    public async Task DeleteAsync(int id)
    {
       await customProductRepository.DeleteAsync(id);
    }

    public async Task<List<ProductGetDTO>> GetAllAsync(int? categoryId = null)
    {
        List<Product> products;

        if(categoryId.HasValue)
        {
            products = await customProductRepository.GetProductsByCategoryIdAsync(categoryId.Value);
        }
        else
        {
            products = await customProductRepository.GetAllAsync();
        }

        var productDTOs = new List<ProductGetDTO>();
        
        foreach (var product in products)
        {
            productDTOs.Add(new ProductGetDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Categories = product.Categories.Select(c => new CategoryGetDTO
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            });
        }

        return productDTOs;
    }

    public async Task<ProductGetDTO> UpdateAsync(int id, ProductGetDTO productDTO, List<int> newCategoryIDs)
    {
        var existingProduct = await customProductRepository.GetProductWithCategoriesAsync(id);
        
        if (existingProduct == null)
        {
            throw new Exception($"Product with id {id} not found");
        }


        existingProduct.Name = productDTO.Name;
        existingProduct.Description = productDTO.Description;
        existingProduct.ImageUrl = productDTO.ImageUrl;
        existingProduct.Price = productDTO.Price;


        existingProduct.Categories.Clear();

        foreach (var categoryId in newCategoryIDs)
        {
            var category = await categoryRepository.GetByIdAsync(categoryId);
            if (category != null)
            {
                existingProduct.Categories.Add(category);
            }
        }
        
        var updatedProduct = await customProductRepository.UpdateAsync(existingProduct);
        return new ProductGetDTO
        {
            Id = updatedProduct.Id,
            Name = updatedProduct.Name,
            Description = updatedProduct.Description,
            ImageUrl = updatedProduct.ImageUrl,
            Price = updatedProduct.Price,
            Categories = updatedProduct.Categories.Select(c => new CategoryGetDTO
            {
                Id = c.Id,
                Name = c.Name
            }).ToList()
        };
    }
}
