namespace AstraVenturaNotebook.Core.Entities;

public class Topic
{
    public string Id { get; private set; }
    public string UserId { get; private set; }
    public string Name { get; private set; }
    public string? ParentId { get; private set; }

    // Para búsqueda rápida
    public string Path { get; private set; }
    public List<string> DnaChain { get; private set; }

    public DateTime CreatedAt { get; private set; }

    // Constructor privado para el ORM/Driver de BD (MongoDB lo requiere al deserializar)
    private Topic() { }

    /// <summary>
    /// Constructor para crear un Tema Raíz (ej. "Matemáticas")
    /// </summary>
    public Topic(string id, string userId, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        Id = id;
        UserId = userId;
        Name = name;
        ParentId = null;

        // La ruta inicial es solo su propio ID
        Path = $"/{id}/";
        DnaChain = new List<string> { name };
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Constructor para crear un Subtema (ej. "Álgebra" dentro de "Matemáticas")
    /// </summary>
    public Topic(string id, string name, Topic parentTopic)
    {
        if (parentTopic == null)
            throw new ArgumentNullException(nameof(parentTopic));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        Id = id;
        UserId = parentTopic.UserId; // Hereda el dueño
        Name = name;
        ParentId = parentTopic.Id;

        // Concatenando la ruta Parent con esta (this)
        Path = $"{parentTopic.Path}{id}/";

        // Copia los ancestros del padre y se agrega a sí mismo
        DnaChain = new List<string>(parentTopic.DnaChain) { name };

        CreatedAt = DateTime.UtcNow;
    }
}
