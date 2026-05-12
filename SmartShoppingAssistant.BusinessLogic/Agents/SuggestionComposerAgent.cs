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
                        1. Search for relevant products based on the items currently in the cart and the available categories.
                        2. Prioritize suggesting products that help activate the "Near-Miss Promotions" (e.g., "Mai adauga 1 produs pentru reducere").
                        3. You MUST return a MAXIMUM of 5 suggestions. Do not exceed this limit under any circumstances.
                        4. Write the "reason" for the suggestion based on the rules above.
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
