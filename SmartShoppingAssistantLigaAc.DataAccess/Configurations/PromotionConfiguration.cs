using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;

namespace SmartShoppingAssistantLigaAc.DataAccess.Configurations;

public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
{
    public void Configure(EntityTypeBuilder<Promotion> builder)
    {
        builder.ToTable("Promotions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);

        builder.Property(p => p.Type).IsRequired();

        builder.Property(p => p.Threshold).IsRequired().HasPrecision(10, 2);

        builder.Property(p => p.Reward).IsRequired();

        builder.Property(p => p.RewardValue).IsRequired();

        builder.HasMany(p => p.Products)
            .WithMany(p => p.Promotions)
            .UsingEntity(j => j.ToTable("PromotionProducts"));

        builder.HasMany(p => p.Categories)
            .WithMany(c => c.Promotions)
            .UsingEntity(j => j.ToTable("PromotionCategories"));
    }
}
