namespace Api.Data.Configurations
{
    using Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MiscDataConfiguration : IEntityTypeConfiguration<MiscData>
    {
        public void Configure(EntityTypeBuilder<MiscData> builder)
        {
            builder.HasKey(d => d.Key);
        }
    }
}
