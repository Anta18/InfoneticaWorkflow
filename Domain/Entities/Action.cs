namespace Domain.Entities
{
    public class Action
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;

        public IReadOnlyCollection<Guid> FromStateIds { get; private set; }

        public Guid ToStateId { get; private set; }
        public bool Enabled { get; private set; } = true;
        internal Guid DefinitionId { get; set; }

        private Action()
        {
            FromStateIds = Array.Empty<Guid>();
        }

        public Action(
            Guid id,
            string name,
            IEnumerable<Guid> fromStates,
            Guid toState,
            bool enabled = true)
        {
            Id = id;
            Name = name;
            FromStateIds = fromStates.ToList().AsReadOnly();
            ToStateId = toState;
            Enabled = enabled;
        }

        public void Disable() => Enabled = false;
        public void Enable() => Enabled = true;
    }
}
