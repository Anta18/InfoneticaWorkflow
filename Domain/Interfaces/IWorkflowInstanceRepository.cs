using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkflowInstanceRepository
    {
        Task<WorkflowInstance?> GetByIdAsync(Guid id);
        Task<IEnumerable<WorkflowInstance>> ListAllAsync();
        Task AddAsync(WorkflowInstance instance);
        Task UpdateAsync(WorkflowInstance instance);
    }
}
