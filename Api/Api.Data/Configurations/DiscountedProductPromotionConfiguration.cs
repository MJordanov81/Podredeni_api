namespace Api.Data.Configurations
{
    using Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DiscountedProductPromotionConfiguration : IEntityTypeConfiguration<DiscountedProductPromotion>
    {
        public void Configure(EntityTypeBuilder<DiscountedProductPromotion> builder)
        {
            builder.HasKey(dp => new { dp.ProductId, dp.PromotionId });
        }
    }
}
