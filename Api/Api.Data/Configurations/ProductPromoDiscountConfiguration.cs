namespace Api.Data.Configurations
{
    using Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductPromoDiscountConfiguration : IEntityTypeConfiguration<ProductPromoDiscount>
    {
        public void Configure(EntityTypeBuilder<ProductPromoDiscount> builder)
        {
            builder.HasKey(p => new { p.ProductId, p.PromoDiscountId });
        }
    }
}
