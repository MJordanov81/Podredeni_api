namespace Api.Data.Configurations
{
    using Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductPromotionConfiguration : IEntityTypeConfiguration<ProductPromotion>
    {
        public void Configure(EntityTypeBuilder<ProductPromotion> builder)
        {
            builder.HasKey(pp => new { pp.ProductId, pp.PromotionId });
        }
    }
}
