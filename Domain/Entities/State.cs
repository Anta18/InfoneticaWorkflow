using System;

namespace Domain.Entities
{
    public class State
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public bool IsStart { get; private set; }
        public bool IsEnd { get; private set; }
        internal Guid DefinitionId { get; set; }

        private State() { }

        public State(Guid id, string name, bool isStart = false, bool isEnd = false)
        {
            Id = id;
            Name = name;
            IsStart = isStart;
            IsEnd = isEnd;
        }
    }
}
