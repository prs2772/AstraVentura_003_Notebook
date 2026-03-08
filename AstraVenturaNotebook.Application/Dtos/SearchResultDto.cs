namespace AstraVenturaNotebook.Application.Dtos;

// DTO para devolver el resultado de la búsqueda
public record SearchResultDto(
    string NoteId,
    string Title,
    string Snippet, // Un fragmento del texto encontrado
    List<string> PathNames // Ej: ["Matemáticas", "Álgebra"]
);
