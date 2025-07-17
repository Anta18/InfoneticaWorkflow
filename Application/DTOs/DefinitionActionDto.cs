namespace Application.DTOs
{
    public record DefinitionActionDto(
        Guid Id,
        string Name,
        IEnumerable<Guid> FromStateIds,
        Guid ToStateId,
        bool Enabled
    );
}
