using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.BusinessLogic.DTOs.Requests;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using SmartShoppingAssistantLigaAc.DataAccess.Repositories.Interfaces;
using SmartShoppingAssistantLigaAc.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class CartItemService(ICartItemRepository CartItemRepository, IProductRepository productRepository, IPromotionRepository promotionRepository) : ICartItemService
{
    public async Task<CartItemGetDTO> AddAsync(CartItemAddRequest request)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId);

        if (product == null)
        {
            throw new Exception($"Product with ID {request.ProductId} not found.");
        }

        var cartItem = new CartItem
        {
            ProductId = request.ProductId,
            Quantity = request.Quantity
        };

        var addedCartItem = await CartItemRepository.AddAsync(cartItem);
        return new CartItemGetDTO
        {
            Id = addedCartItem.Id,
            ProductId = product.Id,
            Quantity = addedCartItem.Quantity,
            Product = new ProductGetDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            }
        };
    }

    public async Task DeleteByIdAsync(int id)
    {
        await CartItemRepository.DeleteAsync(id);
    }

    public async Task DeleteAllAsync()
    {
        var cartItems = await CartItemRepository.GetAllWithProductsAsync();
        foreach (var cartItem in cartItems)
        {
            await CartItemRepository.DeleteAsync(cartItem.Id);
        }
    }

    public async Task<CartGetDTO> GetAsync()
    {
        var cartItems = await CartItemRepository.GetAllWithProductsAsync();
        var cartDTO = new CartGetDTO();
        foreach (var cartItem in cartItems)
        {
            var itemDTO = new CartItemGetDTO
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Product = new ProductGetDTO
                {
                    Id = cartItem.Product.Id,
                    Name = cartItem.Product.Name,
                    Price = cartItem.Product.Price
                }
            };
            
            cartDTO.Items.Add(itemDTO);
            cartDTO.BaseTotal += (cartItem.Quantity * cartItem.Product.Price);
        }

        var activePromotions = await promotionRepository.GetActivePromotionsAsync();
        foreach(var promotion in activePromotions)
        {
            if (promotion.Products.Any())
            {
                var eligibleItems = cartItems.Where(ci => promotion.Products.Any(p => p.Id == ci.ProductId)).ToList();
                foreach (var item in eligibleItems)
                {
                    // Percentage discount off on specific items
                    if (promotion.Type == PromotionType.CartTotal)
                    {
                        decimal percentage = promotion.RewardValue / 100m;
                        decimal itemDiscount = (item.Quantity * item.Product.Price) * percentage;
                        cartDTO.DiscountAmount += itemDiscount;

                        if (!cartDTO.AppliedPromotions.Contains(promotion.Name))
                        {
                            cartDTO.AppliedPromotions.Add(promotion.Name);
                        }
                    }
                    else if (promotion.Type == PromotionType.Quantity)
                    {
                        if (item.Quantity >= promotion.Threshold)
                        {
                            // How many times the threshold is met
                            int triggeredTimes = item.Quantity / (int)promotion.Threshold;

                            // Multiply triggers by the reward quantity to get total free items
                            int freeItems = triggeredTimes * (int)promotion.RewardValue;

                            decimal itemDiscount = freeItems * item.Product.Price;

                            cartDTO.DiscountAmount += itemDiscount;

                            if (!cartDTO.AppliedPromotions.Contains(promotion.Name))
                            {
                                cartDTO.AppliedPromotions.Add(promotion.Name);
                            }
                        }
                    }
                }
            }
            else             // Discount on entire cart total
            {
                if(cartDTO.BaseTotal >= promotion.Threshold)
                {
                    if(promotion.Type == PromotionType.CartTotal)
                    {
                        decimal percentage = promotion.RewardValue / 100m;
                        cartDTO.DiscountAmount += cartDTO.BaseTotal * percentage;
                        if (!cartDTO.AppliedPromotions.Contains(promotion.Name))
                        {
                            cartDTO.AppliedPromotions.Add(promotion.Name);
                        }
                    }
                }
            } 
        }

        cartDTO.FinalTotal = cartDTO.BaseTotal - cartDTO.DiscountAmount;
        if(cartDTO.FinalTotal < 0)
        {
            cartDTO.FinalTotal = 0;     // Negative totals don't make sense, so we set it to zero if discounts exceed the base total
        }

        return cartDTO;
    }

    public async Task<CartItemGetDTO> UpdateAsync(int id, CartItemGetDTO cartItemDTO)
    {
        var existingCartItem = await CartItemRepository.GetByIdWithProductsAsync(id);
        if (existingCartItem == null)
        {
            throw new Exception($"Cart item with ID {id} not found.");
        }
        existingCartItem.Quantity = cartItemDTO.Quantity;
        var updatedCartItem = await CartItemRepository.UpdateAsync(existingCartItem);
        return new CartItemGetDTO
        {
            Id = updatedCartItem.Id,
            ProductId = updatedCartItem.ProductId,
            Quantity = updatedCartItem.Quantity,
            Product = new ProductGetDTO
            {
                Id = updatedCartItem.Product.Id,
                Name = updatedCartItem.Product.Name,
                Price = updatedCartItem.Product.Price
            }
        };
    }
}
