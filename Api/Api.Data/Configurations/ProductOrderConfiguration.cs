namespace Api.Data.Configurations
{
    using Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductOrderConfiguration : IEntityTypeConfiguration<ProductOrder>
    {
        public void Configure(EntityTypeBuilder<ProductOrder> builder)
        {
            //builder.HasKey(po => new { po.ProductId, po.OrderId});
        }
    }
}
