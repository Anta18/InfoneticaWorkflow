namespace Application.Requests
{
    public class CreateDefinitionRequest
    {
        public string Name { get; set; } = null!;
        public IEnumerable<StateDto> States { get; set; } = Array.Empty<StateDto>();
        public IEnumerable<ActionDto> Actions { get; set; } = Array.Empty<ActionDto>();
    }

    public record StateDto(Guid Id, string Name, bool IsStart, bool IsEnd);

    public record ActionDto(
        string Name,
        IEnumerable<Guid> FromStates,
        Guid ToState
    );
}
