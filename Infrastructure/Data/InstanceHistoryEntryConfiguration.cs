using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations
{
    public class InstanceHistoryEntryConfiguration : IEntityTypeConfiguration<InstanceHistoryEntry>
    {
        public void Configure(EntityTypeBuilder<InstanceHistoryEntry> builder)
        {
            builder.ToTable("InstanceHistory");
            builder.HasKey(h => h.Id);

            builder.Property(h => h.InstanceId)
                   .IsRequired();

            builder.Property(h => h.ActionId)
                   .IsRequired();

            builder.Property(h => h.PerformedAt)
                   .IsRequired();
        }
    }
}
