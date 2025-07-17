using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using WorkflowAction = Domain.Entities.Action;
using Infrastructure.Data.Configurations;

namespace Infrastructure.Data
{
    public class WorkflowDbContext : DbContext
    {
        public WorkflowDbContext(DbContextOptions<WorkflowDbContext> options)
            : base(options) { }

        public DbSet<WorkflowDefinition> WorkflowDefinitions { get; set; } = null!;
        public DbSet<State> States { get; set; } = null!;
        public DbSet<WorkflowAction> Actions { get; set; } = null!;
        public DbSet<WorkflowInstance> WorkflowInstances { get; set; } = null!;
        public DbSet<InstanceHistoryEntry> InstanceHistory { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WorkflowDefinitionConfiguration());
            modelBuilder.ApplyConfiguration(new StateConfiguration());
            modelBuilder.ApplyConfiguration(new ActionConfiguration());
            modelBuilder.ApplyConfiguration(new WorkflowInstanceConfiguration());
            modelBuilder.ApplyConfiguration(new InstanceHistoryEntryConfiguration());
        }
    }
}
