using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Storage;

namespace Infrastructure.Repositories
{
    public class WorkflowInstanceRepository : IWorkflowInstanceRepository
    {
        private readonly InMemoryStore<WorkflowInstance> _store;

        public WorkflowInstanceRepository()
        {
            _store = new InMemoryStore<WorkflowInstance>("instances.json");
        }

        public Task AddAsync(WorkflowInstance instance)
            => _store.AddAsync(instance.Id, instance);

        public Task<WorkflowInstance?> GetByIdAsync(Guid id)
            => _store.GetAsync(id);

        public Task<IEnumerable<WorkflowInstance>> ListAllAsync()
            => _store.ListAsync();

        public Task UpdateAsync(WorkflowInstance instance)
            => _store.UpdateAsync(instance.Id, instance);
    }
}
