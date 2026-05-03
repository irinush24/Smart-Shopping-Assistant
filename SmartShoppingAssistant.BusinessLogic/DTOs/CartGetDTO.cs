using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.DTOs;

public class CartGetDTO
{
    public List<CartItemGetDTO> Items { get; set; } = new List<CartItemGetDTO>();
    public decimal BaseTotal { get; set; }
    public decimal DiscountAmount { get; set; }

    public decimal FinalTotal {  get; set; }

    public List<String> AppliedPromotions { get; set; } = new List<String>();
}
