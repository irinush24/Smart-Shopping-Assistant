using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using SmartShoppingAssistant.BusinessLogic.Agents.Interfaces;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.BusinessLogic.Tools;
using System.ComponentModel;

namespace SmartShoppingAssistant.BusinessLogic.Agents;
public class SuggestionComposerAgent(IChatClient chatClient, IProductService productService) : ISuggestionComposerAgent
{
    public ChatClientAgent Build(string cartJson, string categoriesJson)
    {
        return new ChatClientAgent(
            chatClient,
            new ChatClientAgentOptions
            {
                Name = "SuggestionComposer",
                Description = "Composes suggestions based on cart, categories and promotions",
                ChatOptions = new ChatOptions
                {
                    Instructions = $"""
                        You compose shopping suggestions. Here is the current cart:
                        {cartJson}
    
                        Here are the product categories:
                        {categoriesJson}
    
                        Follow these rules strictly when composing suggestions:
                        1. You MUST call the `GetProductsForCategory` tool to find real products to suggest.
                        2. Prioritize suggesting products that help activate "Near-Miss Promotions".
                        3. You MUST populate ALL fields in the JSON response for every suggestion:
                           - `productId`: Use the exact ID of the product.
                           - `name`: Use the exact name of the product.
                           - `price`: Use the exact price of the product.
                           - `quantity`: You MUST calculate (Target Amount - Current Cart Amount). Output ONLY the missing number of items the user still needs to add. (e.g., If the promo requires 5, and they have 1, the quantity MUST be 4).
                        4. Write a clear `reason`
                        5. If a promotion is already fully triggered (meaning required additional quantity is 0), DO NOT generate a suggestion for it. Only generate a suggestions where the user actually needs to add 1 or more items.
                        """,
                    ResponseFormat = ChatResponseFormat.ForJsonSchema<SuggestionList>(),
                    Tools =
                    [
                        AIFunctionFactory.Create(
                            ([Description("The category ID to get products for")] int categoryId) =>
                                ShoppingTools.GetProductsByCategory(categoryId, productService),
                            "GetProductsForCategory",
                            "Get a list of products for a specific category ID."
                        )
                    ]
                }
            },
            null!,
            null!
        );
    }
}
