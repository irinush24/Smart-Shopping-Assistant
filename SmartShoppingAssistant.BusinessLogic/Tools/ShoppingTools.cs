using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.Tools;

public class ShoppingTools
{
    [Description("Get all active promotions that apply to a specific product (by product ID or its category)")]

    public static async Task<List<PromotionGetDTO>> GetPromotionsForProduct(
        [Description("The product ID to check")] int productId, IPromotionService promotionService)
    {
        return await promotionService.GetForProductAsync(productId);
    }

    public static async Task<string> GetProductsByCategory(int categoryId, IProductService productService)
    {
        var products = await productService.GetByCategoryAsync(categoryId);

        return string.Join(", ", products.Select(p => $"{{ Id: {p.Id}, Name: {p.Name}, Price: {p.Price} }}"));
    }
}
