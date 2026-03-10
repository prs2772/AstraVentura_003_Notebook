using AstraVenturaNotebook.Core.Entities;
using AstraVenturaNotebook.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AstraVenturaNotebook.Infrastructure.Persistence;

public class NoteRepository : INoteRepository
{
    private readonly MongoDbContext _context;

    public NoteRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Note?> GetByIdAsync(string id)
    {
        return await _context.Notes.Find(n => n.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Note note)
    {
        await _context.Notes.InsertOneAsync(note);
    }

    public async Task UpdateAsync(Note note)
    {
        await _context.Notes.ReplaceOneAsync(n => n.Id == note.Id, note);
    }

    public async Task DeleteAsync(string id)
    {
        await _context.Notes.DeleteOneAsync(n => n.Id == id);
    }

    public async Task<IEnumerable<Note>> SearchInPathAsync(
        string userId,
        string basePath,
        string searchTerm
    )
    {
        var builder = Builders<Note>.Filter;

        // 1. Condición: Pertenece al usuario
        var userFilter = builder.Eq(n => n.UserId, userId);

        // 2. Condición: La ruta empieza con la ruta del tema padre (Búsqueda recursiva)
        // Al usar "^" al inicio, MongoDB utiliza el índice de manera eficiente.
        var pathFilter = builder.Regex(n => n.TopicPath, new BsonRegularExpression($"^{basePath}"));

        // 3. Condición: El texto existe en el Título o en el Contenido (Case-insensitive)
        var searchRegex = new BsonRegularExpression(searchTerm, "i");
        var textFilter = builder.Or(
            builder.Regex(n => n.Title, searchRegex),
            builder.Regex(n => n.Content, searchRegex)
        );

        // Combinamos todo con AND
        var finalFilter = builder.And(userFilter, pathFilter, textFilter);

        return await _context.Notes.Find(finalFilter).ToListAsync();
    }

    public async Task<IEnumerable<Note>> GetByTopicPathAsync(string userId, string basePath)
    {
        var builder = Builders<Note>.Filter;

        var userFilter = builder.Eq(n => n.UserId, userId);
        var pathFilter = builder.Regex(n => n.TopicPath, new BsonRegularExpression($"^{basePath}"));

        var finalFilter = builder.And(userFilter, pathFilter);

        return await _context.Notes.Find(finalFilter).ToListAsync();
    }
}
