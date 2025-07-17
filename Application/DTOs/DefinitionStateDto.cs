namespace Application.DTOs
{
    public record DefinitionStateDto(
        Guid Id,
        string Name,
        bool IsStart,
        bool IsEnd,
        bool Enabled
    );
}
