using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Requests;

public class CartItemAddRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}