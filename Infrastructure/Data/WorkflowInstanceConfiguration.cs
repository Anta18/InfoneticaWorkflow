using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations
{
    public class WorkflowInstanceConfiguration : IEntityTypeConfiguration<WorkflowInstance>
    {
        public void Configure(EntityTypeBuilder<WorkflowInstance> builder)
        {
            builder.ToTable("WorkflowInstances");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.DefinitionId)
                   .IsRequired();

            builder.Property(i => i.CurrentStateId)
                   .IsRequired();

            builder.Property(i => i.CreatedAt)
                   .IsRequired();

            builder.HasMany(i => i.History)
                   .WithOne()
                   .HasForeignKey(h => h.InstanceId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
