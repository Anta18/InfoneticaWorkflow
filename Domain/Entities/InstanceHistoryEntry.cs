namespace Domain.Entities
{
    public class InstanceHistoryEntry
    {
        public Guid Id { get; private set; }
        public Guid InstanceId { get; private set; }
        public Guid ActionId { get; private set; }
        public DateTime PerformedAt { get; private set; }

        private InstanceHistoryEntry() { }

        public InstanceHistoryEntry(Guid id, Guid instanceId, Guid actionId, DateTime performedAt)
        {
            Id = id;
            InstanceId = instanceId;
            ActionId = actionId;
            PerformedAt = performedAt;
        }
    }
}
