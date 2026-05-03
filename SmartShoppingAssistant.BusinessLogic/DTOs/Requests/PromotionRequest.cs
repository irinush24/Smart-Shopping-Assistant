using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Requests;

public class PromotionRequest
{
    public PromotionGetDTO Promotion { get; set; }
    public List<int> CategoryIDs { get; set; }
    public List<int> ProductIDs { get; set; }
}
