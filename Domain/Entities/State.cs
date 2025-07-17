namespace Domain.Entities
{
    public class State
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public bool IsStart { get; private set; }
        public bool IsEnd { get; private set; }
        public bool Enabled { get; private set; } = true;
        internal Guid DefinitionId { get; set; }

        private State() { }

        public State(
            Guid id,
            string name,
            bool isStart = false,
            bool isEnd = false,
            bool enabled = true)
        {
            Id = id;
            Name = name;
            IsStart = isStart;
            IsEnd = isEnd;
            Enabled = enabled;
        }

        public void Disable() => Enabled = false;
        public void Enable() => Enabled = true;
    }
}
