namespace Domain.Entities
{
    public class WorkflowDefinition
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        private readonly List<State> _states = new();
        private readonly List<Action> _actions = new();

        public IReadOnlyCollection<State> States => _states;
        public IReadOnlyCollection<Action> Actions => _actions;

        private WorkflowDefinition() { }

        public WorkflowDefinition(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddState(State state)
        {
            if (_states.Any(s => s.Id == state.Id))
                throw new InvalidOperationException($"State {state.Name} already added.");
            if (state.IsStart && _states.Any(s => s.IsStart))
                throw new InvalidOperationException("Start state already defined.");
            _states.Add(state);
        }

        public void AddAction(Action action)
        {
            // validate all origins
            foreach (var from in action.FromStateIds)
                if (!_states.Any(s => s.Id == from))
                    throw new InvalidOperationException($"FromState {from} not found.");
            // validate target
            if (!_states.Any(s => s.Id == action.ToStateId))
                throw new InvalidOperationException($"ToState {action.ToStateId} not found.");

            _actions.Add(action);
        }
    }
}
