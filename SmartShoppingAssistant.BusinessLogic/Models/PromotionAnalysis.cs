using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartShoppingAssistant.BusinessLogic.Models;

[Description("Promotion analysis for the current cart")]
public sealed class PromotionAnalysis
{
    [JsonPropertyName("summary")]
    public string Summary { get; set; } = "";

    [JsonPropertyName("suggestions")]
    public List<PromotionSuggestion> Suggestions { get; set; } = [];
}

public sealed class PromotionSuggestion
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; } = "";

    [JsonPropertyName("savings")]
    public decimal? Savings { get; set; }
}