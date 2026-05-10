using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.DTOs;

public class CartGetDTO
{
    public List<CartItemGetDTO> Items { get; set; } = new();
    public decimal Subtotal { get; set; }

    public List<AppliedPromotionDTO> AppliedPromotions { get; set; } = new();

    
    public decimal TotalDiscount { get; set; }
    public decimal Total { get; set; }
}
