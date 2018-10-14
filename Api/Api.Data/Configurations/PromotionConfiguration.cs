using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.HasMany(p => p.ProductsPromotions)
                .WithOne(pp => pp.Promotion)
                .HasForeignKey(pp => pp.PromotionId);

            builder.HasMany(p => p.DiscountedProductsPromotions)
                .WithOne(dp => dp.Promotion)
                .HasForeignKey(dp => dp.PromotionId);
        }
    }
}
