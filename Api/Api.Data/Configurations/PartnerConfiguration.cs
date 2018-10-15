namespace Api.Data.Configurations
{
    using Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PartnerConfiguration : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder
                .HasMany(p => p.PartnerLocations)
                .WithOne(pl => pl.Partner)
                .HasForeignKey(pl => pl.PartnerId);
        }
    }
}
