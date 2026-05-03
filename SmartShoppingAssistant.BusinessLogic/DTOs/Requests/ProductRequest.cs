using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Requests;

public class ProductRequest
{
    public ProductGetDTO Product { get; set; }
    public List<int> NewCategoryIDs { get; set; }
}
