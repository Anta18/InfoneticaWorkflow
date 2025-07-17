using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Storage;

namespace Infrastructure.Repositories
{
    public class WorkflowDefinitionRepository : IWorkflowDefinitionRepository
    {
        // private readonly WorkflowDbContext _db;
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
    }
}
