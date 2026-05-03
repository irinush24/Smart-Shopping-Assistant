using System;
using System.Collections.Generic;
using System.Text;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using SmartShoppingAssistantLigaAc.DataAccess.Repositories.Interfaces;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class PromotionService (IPromotionRepository promotionRepository, IRepository<Category> categoryRepository, IProductRepository productRepository): IPromotionService
{
    public async Task<PromotionGetDTO> AddAsync(PromotionGetDTO promotionDTO, List<int>categoryIds, List<int>productIds)
    {
        var promotion = new Promotion
        {
            Id = promotionDTO.Id,
            Name = promotionDTO.Name,
            Type = promotionDTO.Type,
            Threshold = promotionDTO.Threshold,
            Reward = promotionDTO.Reward,
            RewardValue = promotionDTO.RewardValue,
            IsActive = promotionDTO.IsActive
        };
        foreach(var categoryId in categoryIds)
        {
            var category = await categoryRepository.GetByIdAsync(categoryId);
            if(category != null)
            {
                promotion.Categories.Add(category);
            }
        }
        foreach(var productId in productIds)
        {
            var product = await productRepository.GetByIdAsync(productId);
            if(product != null)
            {
                promotion.Products.Add(product);
            }
        }
        var addedPromotion = await promotionRepository.AddAsync(promotion);
        return new PromotionGetDTO
        {
            Id = addedPromotion.Id,
            Name = addedPromotion.Name,
            Type = addedPromotion.Type,
            Threshold = addedPromotion.Threshold,
            Reward= addedPromotion.Reward,
            RewardValue= addedPromotion.RewardValue,
            IsActive= addedPromotion.IsActive
        };
    }

    public async Task<PromotionGetDTO> GetByIdAsync(int id)
    {
        var promotion = await promotionRepository.GetByIdAsync(id);
        if (promotion == null)
        {
            throw new Exception($"Promotion with ID {id} not found.");
        }
        return new PromotionGetDTO
        {
            Id = promotion.Id,
            Name = promotion.Name,
            Type = promotion.Type,
            Threshold = promotion.Threshold,
            Reward = promotion.Reward,
            RewardValue = promotion.RewardValue,
            IsActive = promotion.IsActive
        };
    }

    public async Task DeleteAsync(int id)
    {
        await promotionRepository.DeleteAsync(id);
    }

    public async Task<List<PromotionGetDTO>> GetAllAsync()
    {
        var promotions = await promotionRepository.GetAllAsync();
        var promotionDTOs = new List<PromotionGetDTO>();
        foreach (var promotion in promotions)
        {
            promotionDTOs.Add(new PromotionGetDTO
            {
                Id = promotion.Id,
                Name = promotion.Name,
                Type = promotion.Type,
                Threshold = promotion.Threshold,
                Reward = promotion.Reward,
                RewardValue = promotion.RewardValue,
                IsActive = promotion.IsActive
            });
        }
        return promotionDTOs;
    }

    public async Task<PromotionGetDTO> UpdateAsync(int id, PromotionGetDTO promotionDTO, List<int> newCategoryID, List<int> newProductID)
    {
        var existingPromotion = await promotionRepository.GetPromotionByIdAsync(id);
        if (existingPromotion == null)
        {
            throw new Exception($"Promotion with ID {id} not found.");
        }
        existingPromotion.Name = promotionDTO.Name;
        existingPromotion.Type = promotionDTO.Type;
        existingPromotion.Threshold = promotionDTO.Threshold;
        existingPromotion.Reward = promotionDTO.Reward;
        existingPromotion.RewardValue = promotionDTO.RewardValue;
        existingPromotion.IsActive = promotionDTO.IsActive;
        existingPromotion.Categories.Clear();
        existingPromotion.Products.Clear();
        var categoriesToRemove = existingPromotion.Categories.Where(c => !newCategoryID.Contains(c.Id)).ToList();

        foreach (var cat in categoriesToRemove)
        {
            existingPromotion.Categories.Remove(cat);
        }

        foreach(var categoryId in newCategoryID)
        {
            if(!existingPromotion.Categories.Any(c => c.Id == categoryId))
            {
                var category = await categoryRepository.GetByIdAsync(categoryId);
                if(category != null)
                {
                    existingPromotion.Categories.Add(category);
                }
            }
        }

        var productsToRemove = existingPromotion.Products.Where(p => !newProductID.Contains(p.Id)).ToList();
        foreach (var product in productsToRemove)
        {
            existingPromotion.Products.Remove(product);
        }

        foreach (var productId in newProductID)
        {
            if(!existingPromotion.Products.Any(p => p.Id == productId))
            {
                var product = await productRepository.GetByIdAsync(productId);
                if (product != null)
                {
                    existingPromotion.Products.Add(product);
                }
            }
        }

        var updatedPromotion = await promotionRepository.UpdateAsync(existingPromotion);

        return new PromotionGetDTO
        {
            Id = updatedPromotion.Id,
            Name = updatedPromotion.Name,
            Type = updatedPromotion.Type,
            Threshold = updatedPromotion.Threshold,
            Reward = updatedPromotion.Reward,
            RewardValue = updatedPromotion.RewardValue,
            IsActive = updatedPromotion.IsActive
        };
    }
}