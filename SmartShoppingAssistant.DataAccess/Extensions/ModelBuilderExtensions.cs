using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // 1. Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Gadgets and tech devices" },
                new Category { Id = 2, Name = "Groceries", Description = "Daily food and essentials" },
                new Category { Id = 3, Name = "Clothing", Description = "Apparel and accessories" }
            );

            // 2. Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Wireless Mouse", Description = "Ergonomic wireless mouse", Price = 25.99m, ImageUrl = "" },
                new Product { Id = 2, Name = "Gaming Laptop", Description = "High performance laptop", Price = 1299.99m, ImageUrl = "" },
                new Product { Id = 3, Name = "Whole Wheat Bread", Description = "Freshly baked", Price = 3.49m, ImageUrl = "" },
                new Product { Id = 4, Name = "Apples", Description = "1 lb of Fuji apples", Price = 4.99m, ImageUrl = "" }
            );

            // 3. Seed the Many-to-Many Relationship (ProductCategories)
            // Because EF Core created this join table implicitly via UsingEntity("ProductCategories"),
            // we seed it by passing anonymous objects with the exact foreign key column names.
            modelBuilder.Entity("ProductCategories").HasData(
                new { CategoriesId = 1, ProductsId = 1 }, // Wireless Mouse -> Electronics
                new { CategoriesId = 1, ProductsId = 2 }, // Gaming Laptop -> Electronics
                new { CategoriesId = 2, ProductsId = 3 }, // Bread -> Groceries
                new { CategoriesId = 2, ProductsId = 4 }  // Apples -> Groceries
            );

            // 4. Seed Promotions
            // Note: Replace the 'Type = 0' or 'Type = 1' with your actual Enum values if needed
            modelBuilder.Entity<Promotion>().HasData(
                new Promotion
                {
                    Id = 1,
                    Name = "15% off Laptops",
                    Type = 0, // Assuming 0 is PercentageOff
                    Threshold = 0m,
                    Reward = (Entities.Enums.PromotionReward) 0,
                    RewardValue = 15,
                    ProductId = 2, // Links directly to the Laptop
                    CategoryId = null
                },
                new Promotion
                {
                    Id = 2,
                    Name = "Buy 2 Apples Get 1 Free",
                    Type = (Entities.Enums.PromotionType)1, // Assuming 1 is FreeItems
                    Threshold = 2m,
                    Reward = (Entities.Enums.PromotionReward) 1,
                    RewardValue = 0,
                    ProductId = 4, // Links directly to Apples
                    CategoryId = null
                },
                new Promotion
                {
                    Id = 3,
                    Name = "10% off all Groceries over $50",
                    Type = 0,
                    Threshold = 50.00m,
                    Reward = (Entities.Enums.PromotionReward) 0,
                    RewardValue = 10,
                    ProductId = null,
                    CategoryId = 2 // Links to the Grocery Category
                }
            );

            // 5. Seed Cart Items (Optional)
            // It's usually best to leave Cart Items empty so testing starts with a clean slate,
            // but here is how you would seed one if you wanted to test your cart GET logic immediately:
            /*modelBuilder.Entity<CartItem>().HasData(
                new CartItem { Id = 1, ProductId = 1, Quantity = 2 }
            );*/
        }
    }
}