using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkflowDefinitionRepository : IWorkflowDefinitionRepository
    {
        private readonly WorkflowDbContext _db;
        public WorkflowDefinitionRepository(WorkflowDbContext db) => _db = db;
        public Task AddAsync(WorkflowDefinition def)
            => _db.WorkflowDefinitions.AddAsync(def).AsTask();
        public Task<WorkflowDefinition?> GetByIdAsync(Guid id)
            => _db.WorkflowDefinitions
                  .Include(w => w.States)
                  .Include(w => w.Actions)
                  .SingleOrDefaultAsync(w => w.Id == id);
        public Task<IEnumerable<WorkflowDefinition>> ListAllAsync()
            => _db.WorkflowDefinitions
                  .Include(w => w.States)
                  .Include(w => w.Actions)
                  .ToListAsync()
                  .ContinueWith(t => (IEnumerable<WorkflowDefinition>)t.Result);
    }
}
