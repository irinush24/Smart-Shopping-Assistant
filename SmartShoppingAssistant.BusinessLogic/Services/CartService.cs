using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.DataAccess.Repositories;
using System.Text.Json;
using SmartShoppingAssistant.BusinessLogic.Agents.Interfaces;
using SmartShoppingAssistant.BusinessLogic.Agents;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class CartService(ICartItemRepository cartItemRepository, IProductRepository productRepository, IPromotionService promotionService, ICategoryRepository categoryRepository, IPromotionCheckerAgent promotionCheckerAgent, ISuggestionComposerAgent suggestionComposerAgent) : ICartService
{
    public async Task<CartGetDTO> GetCartAsync()
    {
        var cartItems = await cartItemRepository.GetAllWithProductAsync();
        var cartDTO = new CartGetDTO();
        decimal totalDiscountPositive = 0;

        // Base subtotal calculation
        foreach (var cartItem in cartItems)
        {
            var itemSubtotal = cartItem.Product.Price * cartItem.Quantity;
            cartDTO.Items.Add(new CartItemGetDTO
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.Name,
                UnitPrice = cartItem.Product.Price,
                Quantity = cartItem.Quantity,
                Subtotal = itemSubtotal
            });
            cartDTO.Subtotal += itemSubtotal;
        }

        // Fetch all active promotions and apply them
        var allPromotions = await promotionService.GetAllAsync();
        var activePromotions = allPromotions.Where(p => p.IsActive).ToList();

        foreach (var promo in activePromotions)
        {
            // Product-specific promotions
            if (promo.ProductId.HasValue)
            {
                var item = cartItems.FirstOrDefault(i => i.ProductId == promo.ProductId.Value);

                if (item != null)
                {
                    bool isTriggered = false;
                    int triggerMultiplier = 1;

                    // Check if promotion is triggered based on type
                    if (promo.Type == PromotionType.Quantity && item.Quantity >= promo.Threshold)
                    {
                        isTriggered = true;
                        triggerMultiplier = item.Quantity / (int)promo.Threshold;
                    }
                    else if (promo.Type == PromotionType.CartTotal)
                    {
                        var itemTotalSpent = item.Product.Price * item.Quantity;
                        if(itemTotalSpent >= promo.Threshold)
                        {
                            isTriggered = true;
                            triggerMultiplier = (int)(itemTotalSpent / promo.Threshold);
                        }
                    }

                    // Apply the reward
                    if(isTriggered)
                    {
                        decimal itemDiscount = 0;
                        if(promo.Reward == PromotionReward.PercentDiscount)
                        {
                            decimal percentage = promo.RewardValue / 100m;
                            itemDiscount = (item.Product.Price * item.Quantity) * percentage;
                        }
                        else if(promo.Reward == PromotionReward.FreeItems)
                        {
                            int freeItemsToGive = triggerMultiplier * (int)promo.RewardValue;
                            itemDiscount = freeItemsToGive * item.Product.Price;
                        }

                        totalDiscountPositive += itemDiscount;
                        cartDTO.AppliedPromotions.Add(new AppliedPromotionDTO
                        {
                            PromotionName = promo.Name,
                            DiscountAmount = -itemDiscount
                        });
                    }
                }
            }
            // Global cart promotion (no product/category filter)
            else if (promo.ProductId == null && promo.CategoryId == null)
            {
                bool isTriggered = false;

                // Check the trigger
                if(promo.Type == PromotionType.CartTotal && cartDTO.Subtotal >= promo.Threshold)
                {
                    isTriggered = true;
                }
                else if(promo.Type == PromotionType.Quantity)
                {
                    int totalQuantity = cartItems.Sum(i => i.Quantity);
                    if(totalQuantity >= promo.Threshold)
                    {
                        isTriggered = true;
                    }
                }

                if(isTriggered)
                {
                    if(promo.Reward == PromotionReward.PercentDiscount)
                    {
                        decimal percentage = promo.RewardValue / 100m;
                        decimal cartDiscount = cartDTO.Subtotal * percentage;
                        
                        totalDiscountPositive += cartDiscount;
                        cartDTO.AppliedPromotions.Add(new AppliedPromotionDTO
                        {
                            PromotionName = promo.Name,
                            DiscountAmount = -cartDiscount
                        });
                    }
                }
            }
        }

        // Finalize totals
        cartDTO.TotalDiscount = -totalDiscountPositive;
        cartDTO.Total = cartDTO.Subtotal - totalDiscountPositive;

        // Ensure total doesn't go negative
        if (cartDTO.Total < 0) cartDTO.Total = 0;

        return cartDTO;
    }

    public async Task<CartGetDTO> AddItemAsync(AddCartItemDTO dto)
    {
        await productRepository.GetByIdAsync(dto.ProductId); // throws if not found

        var existing = await cartItemRepository.GetByProductIdAsync(dto.ProductId);
        if (existing != null)
        {
            existing.Quantity += dto.Quantity;
            await cartItemRepository.UpdateAsync(existing);
            return await GetCartAsync();
        }

        var item = new CartItem { ProductId = dto.ProductId, Quantity = dto.Quantity };
        await cartItemRepository.AddAsync(item);
        var added = await cartItemRepository.GetByIdWithProductAsync(item.Id);
        return await GetCartAsync();
    }

    public async Task<CartGetDTO> UpdateItemAsync(int itemId, UpdateCartItemDTO dto)
    {
        var item = await cartItemRepository.GetByIdWithProductAsync(itemId);
        item.Quantity = dto.Quantity;
        await cartItemRepository.UpdateAsync(item);
        return await GetCartAsync();
    }

    public async Task<CartGetDTO> RemoveItemAsync(int itemId)
    {
        await cartItemRepository.DeleteAsync(itemId);
        return await GetCartAsync();
    }

    public Task ClearCartAsync() => cartItemRepository.ClearAsync();

    public async Task<PromotionAnalysis> AnalyzeCartAsync()
    {
        var cart = await cartItemRepository.GetAllWithProductsWithCategoriesAsync();
        var categories = await categoryRepository.GetAllAsync();

        var cartJson = JsonSerializer.Serialize(cart.Select(c => new
        {
            c.ProductId,
            ProductName = c.Product.Name,
            c.Product.Price,
            c.Quantity,
            LineTotal = c.Product.Price * c.Quantity,
            Category = c.Product.Categories.Select(cat => new { CategoryId = cat.Id, CategoryName = cat.Name }).ToList()
        }));

        var categoryJson = JsonSerializer.Serialize(categories.Select(c => new { CategoryId = c.Id, CategoryName = c.Name }));

        var promotionAgent = promotionCheckerAgent.Build(cartJson);
        var suggestionAgent = suggestionComposerAgent.Build(cartJson, categoryJson);
        var workflow = new WorkflowBuilder(promotionAgent).AddEdge(promotionAgent, suggestionAgent).WithOutputFrom(suggestionAgent).Build();

        var chatMessage = new List<ChatMessage>
        { 
            new(ChatRole.User, "Analyze the cart and suggest improvements.")
        };

        await using var result = await InProcessExecution.RunStreamingAsync(workflow, chatMessage);

        await result.TrySendMessageAsync(new TurnToken(emitEvents: true));
        
        var jsonBuilder = new System.Text.StringBuilder();

        await foreach (var message in result.WatchStreamAsync())
        {
            if(message is AgentResponseUpdateEvent update && update.ExecutorId.StartsWith("SuggestionComposer"))
            {
                jsonBuilder.Append(update.Update.Text);
            }
            else if(message is WorkflowErrorEvent errorEvent)
            {
                throw new InvalidOperationException(errorEvent.Exception?.Message);
            }
        }

        var json = jsonBuilder.ToString();
        return JsonSerializer.Deserialize<PromotionAnalysis>(json) ?? throw new InvalidOperationException("Failed to deserialize promotion analysis.");
    }
}