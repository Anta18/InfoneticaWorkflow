using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkflowInstanceRepository : IWorkflowInstanceRepository
    {
        private readonly WorkflowDbContext _db;
        public WorkflowInstanceRepository(WorkflowDbContext db) => _db = db;
        public Task AddAsync(WorkflowInstance inst)
            => _db.WorkflowInstances.AddAsync(inst).AsTask();
        public Task<WorkflowInstance?> GetByIdAsync(Guid id)
            => _db.WorkflowInstances
                  .Include(i => i.History)
                  .SingleOrDefaultAsync(i => i.Id == id);
        public Task<IEnumerable<WorkflowInstance>> ListAllAsync()
            => _db.WorkflowInstances
                  .ToListAsync()
                  .ContinueWith(t => (IEnumerable<WorkflowInstance>)t.Result);
        public Task UpdateAsync(WorkflowInstance inst)
        {
            _db.WorkflowInstances.Update(inst);
            return _db.SaveChangesAsync();
        }
    }
}
