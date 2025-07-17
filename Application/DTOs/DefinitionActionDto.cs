namespace Application.DTOs
{
    public record DefinitionActionDto(
        Guid Id,
        string Name,
        Guid FromStateId,
        Guid ToStateId
    );
}
