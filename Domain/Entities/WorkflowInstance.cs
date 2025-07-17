using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class WorkflowInstance
    {
        public Guid Id { get; private set; }
        public Guid DefinitionId { get; private set; }
        public Guid CurrentStateId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        private readonly List<InstanceHistoryEntry> _history = new();
        public IReadOnlyCollection<InstanceHistoryEntry> History => _history;

        private WorkflowInstance() { }

        public WorkflowInstance(WorkflowDefinition definition)
        {
            Id = Guid.NewGuid();
            DefinitionId = definition.Id;
            var start = definition.States.SingleOrDefault(s => s.IsStart)
                       ?? throw new InvalidOperationException("No start state defined.");
            CurrentStateId = start.Id;
            CreatedAt = DateTime.UtcNow;
        }

        public void ExecuteAction(Action action, IEnumerable<State> states)
        {
            if (!action.Enabled)
                throw new InvalidOperationException("Action is disabled.");

            var currentState = states.SingleOrDefault(s => s.Id == CurrentStateId)
                ?? throw new InvalidOperationException("Current state not found.");
            if (!currentState.Enabled)
                throw new InvalidOperationException("Current state is disabled.");

            if (action.FromStateId != CurrentStateId)
                throw new InvalidOperationException("Invalid action for current state.");

            CurrentStateId = action.ToStateId;
            _history.Add(new InstanceHistoryEntry(Guid.NewGuid(), Id, action.Id, DateTime.UtcNow));
        }
    }
}