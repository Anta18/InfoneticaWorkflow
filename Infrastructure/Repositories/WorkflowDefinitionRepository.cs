using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Storage;

namespace Infrastructure.Repositories
{
    public class WorkflowDefinitionRepository : IWorkflowDefinitionRepository
    {
        private readonly InMemoryStore<WorkflowDefinition> _store;

        public WorkflowDefinitionRepository()
        {
            _store = new InMemoryStore<WorkflowDefinition>("definitions.json");
        }

        public Task AddAsync(WorkflowDefinition definition)
            => _store.AddAsync(definition.Id, definition);

        public Task<WorkflowDefinition?> GetByIdAsync(Guid id)
            => _store.GetAsync(id);

        public Task<IEnumerable<WorkflowDefinition>> ListAllAsync()
            => _store.ListAsync();

        public Task UpdateAsync(WorkflowDefinition definition)
            => _store.UpdateAsync(definition.Id, definition);
    }
}
