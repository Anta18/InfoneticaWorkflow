namespace Application.DTOs
{
    public class WorkflowInstanceDto
    {
        public Guid InstanceId { get; init; }
        public Guid CurrentStateId { get; init; }
        public DateTime CreatedAt { get; init; }
        public IEnumerable<HistoryEntryDto> History { get; init; } = Array.Empty<HistoryEntryDto>();
    }
}
