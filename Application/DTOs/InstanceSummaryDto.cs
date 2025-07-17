namespace Application.DTOs
{
    public class InstanceSummaryDto
    {
        public Guid InstanceId { get; init; }
        public Guid DefinitionId { get; init; }
        public Guid CurrentStateId { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
