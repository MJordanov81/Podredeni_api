namespace Api.Data.Configurations
{
    using Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PromoDiscountConfiguration : IEntityTypeConfiguration<PromoDiscount>
    {
        public void Configure(EntityTypeBuilder<PromoDiscount> builder)
        {
            builder.HasMany(d => d.ProductPromoDiscounts).WithOne(p => p.PromoDiscount).HasForeignKey(p => p.PromoDiscountId);
        }
    }
}
