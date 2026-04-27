using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using SmartShoppingAssistantLigaAc.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class ProductService(IRepository<Product> productRepository): IProductService
{
    public async Task<ProductGetDTO> GetByIdAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);

        return new ProductGetDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            Price = product.Price
        };
    }

    public async Task<ProductGetDTO> AddAsync(ProductGetDTO productDTO)
    {
        var product = new Product
        {
            Name = productDTO.Name,
            Description = productDTO.Description,
            ImageUrl = productDTO.ImageUrl,
            Price = productDTO.Price
        };
        var addedProduct = await productRepository.AddAsync(product);
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
        await productRepository.DeleteAsync(id);
    }

    public async Task<List<ProductGetDTO>> GetAllAsync()
    {
        var products = await productRepository.GetAllAsync();

        var productDTOs = new List<ProductGetDTO>();
        
        foreach (var product in products)
        {
            productDTOs.Add(new ProductGetDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price
            });
        }

        return productDTOs;
    }

    public async Task<ProductGetDTO> UpdateAsync(int id, ProductGetDTO productDTO)
    {
        var existingProduct = await productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            throw new Exception($"Product with id {id} not found");
        }
        existingProduct.Name = productDTO.Name;
        existingProduct.Description = productDTO.Description;
        existingProduct.ImageUrl = productDTO.ImageUrl;
        existingProduct.Price = productDTO.Price;
        var updatedProduct = await productRepository.UpdateAsync(existingProduct);
        return new ProductGetDTO
        {
            Id = updatedProduct.Id,
            Name = updatedProduct.Name,
            Description = updatedProduct.Description,
            ImageUrl = updatedProduct.ImageUrl,
            Price = updatedProduct.Price
        };
    }
}
