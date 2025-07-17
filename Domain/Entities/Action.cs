using System;

namespace Domain.Entities
{
    public class Action
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Guid FromStateId { get; private set; }
        public Guid ToStateId { get; private set; }
        public bool Enabled { get; private set; } = true;
        internal Guid DefinitionId { get; set; }

        private Action() { }

        public Action(Guid id, string name, Guid from, Guid to, bool enabled = true)
        {
            Id = id;
            Name = name;
            FromStateId = from;
            ToStateId = to;
            Enabled = enabled;
        }
    }
}