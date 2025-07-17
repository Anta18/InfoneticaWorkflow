using Application.DTOs;

namespace Application.Services
{
    public interface IWorkflowService
    {
        Task<Guid> CreateDefinitionAsync(string name, IEnumerable<(string name, bool isStart, bool isEnd)> states, IEnumerable<(string name, Guid from, Guid to)> actions);
        Task<Guid> StartInstanceAsync(Guid definitionId);
        Task PerformActionAsync(Guid instanceId, Guid actionId);
        Task<WorkflowInstanceDto> GetInstanceAsync(Guid instanceId);
        Task<IEnumerable<WorkflowDefinitionDto>> ListDefinitionsAsync();
        Task<WorkflowDefinitionDto> GetDefinitionAsync(Guid definitionId);
        Task<IEnumerable<InstanceSummaryDto>> ListInstancesAsync();
    }
}
