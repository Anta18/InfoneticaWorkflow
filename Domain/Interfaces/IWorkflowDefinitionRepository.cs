using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkflowDefinitionRepository
    {
        Task<WorkflowDefinition?> GetByIdAsync(Guid id);
        Task<IEnumerable<WorkflowDefinition>> ListAllAsync();
        Task AddAsync(WorkflowDefinition definition);
    }
}
