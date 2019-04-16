namespace Api.Data.Configurations
{
    using Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SubcategoryProductConfiguration : IEntityTypeConfiguration<SubcategoryProduct>
    {
        public void Configure(EntityTypeBuilder<SubcategoryProduct> builder)
        {
            builder.HasKey(sp => new { sp.SubcategoryId, sp.ProductId });
        }
    }
}
