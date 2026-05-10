using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class CartService(ICartItemRepository cartItemRepository, IProductRepository productRepository, IPromotionService promotionService) : ICartService
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
}