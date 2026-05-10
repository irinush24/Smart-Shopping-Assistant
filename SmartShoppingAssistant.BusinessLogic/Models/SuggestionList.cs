using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SmartShoppingAssistant.BusinessLogic.Models;

[Description("List of suggestions based on cart, categories and promotions")]
public sealed class SuggestionList
{
    [JsonPropertyName("suggestions")]
    public List<Suggestion> Suggestions { get; set; } = [];
}

public sealed class Suggestion
{
    [JsonPropertyName("productId")]
    public string ProductId { get; set; }

    [JsonPropertyName("productName")]
    public string ProductName { get; set; } = string.Empty;

    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;

    [JsonPropertyName("categoryId")]
    public int CategoryId { get; set; }
}
