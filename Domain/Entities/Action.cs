using System;

namespace Domain.Entities
{
    public class Action
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Guid FromStateId { get; private set; }
        public Guid ToStateId { get; private set; }
        internal Guid DefinitionId { get; set; }

        private Action() { }

        public Action(Guid id, string name, Guid from, Guid to)
        {
            Id = id;
            Name = name;
            FromStateId = from;
            ToStateId = to;
        }
    }
}
