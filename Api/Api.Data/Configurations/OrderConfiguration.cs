namespace Api.Data.Configurations
{
    using Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasAlternateKey(o => o.Number);

            builder.HasMany(o => o.ProductOrders).WithOne(po => po.Order).HasForeignKey(po => po.OrderId);

            builder.HasMany(o => o.OrderLogs).WithOne(ol => ol.Order).HasForeignKey(ol => ol.OrderId);
        }
    }
}
