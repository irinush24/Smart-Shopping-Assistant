using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SmartShoppingAssistant.BusinessLogic.Models;

[Description("List of suggestions based on cart, categories and promotions")]
public sealed class SuggestionList
{
    [JsonPropertyName("suggestions")]
    public List<PromotionSuggestion> Suggestions { get; set; } = [];
}
