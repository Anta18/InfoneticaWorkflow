using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations
{
    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.ToTable("States");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.IsStart)
                   .IsRequired();

            builder.Property(s => s.IsEnd)
                   .IsRequired();

            builder.Property(s => s.Enabled)
                    .IsRequired();

            builder.Property<Guid>("DefinitionId");
            builder.HasIndex("DefinitionId");
        }
    }
}
