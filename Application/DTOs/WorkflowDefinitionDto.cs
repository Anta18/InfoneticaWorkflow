namespace Application.DTOs
{
    public class WorkflowDefinitionDto
    {
        public Guid DefinitionId { get; init; }
        public string Name { get; init; } = null!;
        public IEnumerable<DefinitionStateDto> States { get; init; } = Array.Empty<DefinitionStateDto>();
        public IEnumerable<DefinitionActionDto> Actions { get; init; } = Array.Empty<DefinitionActionDto>();
    }
}
