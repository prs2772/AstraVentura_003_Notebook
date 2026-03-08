namespace AstraVenturaNotebook.Core.Entities;

public class Note
{
    public string Id { get; private set; }
    public string UserId { get; private set; }
    public string TopicId { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }

    // Denormalizamos el Path y Ancestors para consultas extremadamente rápidas
    public string TopicPath { get; private set; }
    public List<string> TopicAncestors { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Note() { }

    public Note(string id, string title, string content, Topic parentTopic)
    {
        if (parentTopic == null)
            throw new ArgumentNullException(nameof(parentTopic));

        Id = id;
        UserId = parentTopic.UserId;
        TopicId = parentTopic.Id;
        Title = title;
        Content = content;

        // Guardamos la ruta y ancestros en el momento de creación
        TopicPath = parentTopic.Path;
        TopicAncestors = new List<string>(parentTopic.DnaChain);

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    // Único método para modificar el contenido, protegiendo las demás propiedades
    public void UpdateContent(string newTitle, string newContent)
    {
        Title = newTitle;
        Content = newContent;
        UpdatedAt = DateTime.UtcNow;
    }
}
