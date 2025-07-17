using Application.DTOs;

namespace Application.Services
{
    public interface IWorkflowService
    {
        Task<Guid> CreateDefinitionAsync(
            string name,
            IEnumerable<(Guid id, string name, bool isStart, bool isEnd)> states,
            IEnumerable<(string name, IEnumerable<Guid> fromStates, Guid toState)> actions
        );

        Task<Guid> StartInstanceAsync(Guid definitionId);
        Task PerformActionAsync(Guid instanceId, Guid actionId);
        Task<WorkflowInstanceDto> GetInstanceAsync(Guid instanceId);
        Task<IEnumerable<WorkflowDefinitionDto>> ListDefinitionsAsync();
        Task<WorkflowDefinitionDto> GetDefinitionAsync(Guid definitionId);
        Task<IEnumerable<InstanceSummaryDto>> ListInstancesAsync();
        Task DisableStateAsync(Guid definitionId, Guid stateId);
        Task EnableStateAsync(Guid definitionId, Guid stateId);
        Task DisableActionAsync(Guid definitionId, Guid actionId);
        Task EnableActionAsync(Guid definitionId, Guid actionId);
    }
}
