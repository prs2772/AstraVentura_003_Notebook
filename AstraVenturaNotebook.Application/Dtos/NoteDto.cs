namespace AstraVenturaNotebook.Application.Dtos;

// DTO para la creación de una nota
public record NoteDto(string Id, string Title, string Content, List<string> TopicAncestors);
