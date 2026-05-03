using System;
using System.Collections.Generic;
using System.Text;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using SmartShoppingAssistantLigaAc.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.BusinessLogic.DTOs;

public class PromotionGetDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public PromotionType Type { get; set; }
    public decimal Threshold { get; set; }
    public PromotionReward Reward { get; set; }
    public int RewardValue { get; set; }
    public bool IsActive { get; set; }
}
