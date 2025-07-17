using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations
{
    public class WorkflowDefinitionConfiguration : IEntityTypeConfiguration<WorkflowDefinition>
    {
        public void Configure(EntityTypeBuilder<WorkflowDefinition> builder)
        {
            builder.ToTable("WorkflowDefinitions");
            builder.HasKey(w => w.Id);
            builder.Property(w => w.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasMany(w => w.States)
                   .WithOne()
                   .HasForeignKey("DefinitionId");

            builder.HasMany(w => w.Actions)
                   .WithOne()
                   .HasForeignKey("DefinitionId");

        }
    }
}
