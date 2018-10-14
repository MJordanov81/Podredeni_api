namespace Api.Data.Configurations
{
    using Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasAlternateKey(p => p.Number);

            builder.HasMany(p => p.Images).WithOne(i => i.Product).HasForeignKey(i => i.ProductId);

            builder.HasMany(p => p.ProductOrders).WithOne(po => po.Product).HasForeignKey(po => po.ProductId);

            builder.HasMany(p => p.CategoryProducts).WithOne(cp => cp.Product).HasForeignKey(cp => cp.ProductId);

            builder.HasMany(p => p.ProductPromoDiscounts).WithOne(d => d.Product).HasForeignKey(d => d.ProductId);

            builder.HasMany(p => p.ProductsPromotions).WithOne(pp => pp.Product).HasForeignKey(pp => pp.ProductId);

            builder.HasMany(p => p.DiscountedProductsPromotions).WithOne(dp => dp.Product).HasForeignKey(dp => dp.ProductId);
        }
    }
}
