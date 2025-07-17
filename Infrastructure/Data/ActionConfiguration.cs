using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

using WorkflowAction = Domain.Entities.Action;

namespace Infrastructure.Data.Configurations
{
    public class ActionConfiguration : IEntityTypeConfiguration<WorkflowAction>
    {
        public void Configure(EntityTypeBuilder<WorkflowAction> builder)
        {
            builder.ToTable("Actions");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property<Guid>("DefinitionId");
            builder.HasIndex("DefinitionId");

            builder.Property(a => a.FromStateId).IsRequired();
            builder.Property(a => a.ToStateId).IsRequired();
            builder.Property(a => a.Enabled).IsRequired();

            builder.HasOne<State>()
                   .WithMany()
                   .HasForeignKey(a => a.FromStateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<State>()
                   .WithMany()
                   .HasForeignKey(a => a.ToStateId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
