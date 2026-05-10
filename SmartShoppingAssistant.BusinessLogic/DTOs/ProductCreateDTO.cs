namespace SmartShoppingAssistant.BusinessLogic.DTOs;

public class ProductCreateDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public decimal Price { get; set; }

    public List<int> CategoryIds { get; set; } = new();
}
