using AstraVenturaNotebook.Core.Entities;
using AstraVenturaNotebook.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AstraVenturaNotebook.Infrastructure.Persistence;

public class TopicRepository : ITopicRepository
{
    private readonly MongoDbContext _context;

    public TopicRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Topic?> GetByIdAsync(string id)
    {
        return await _context.Topics.Find(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Topic>> GetSubtopicsAsync(string userId, string? parentTopicId)
    {
        // Trae los temas del usuario actual que coincidan con el padre solicitado
        // (si parentTopicId es null, traerá los temas raíz)
        return await _context
            .Topics.Find(t => t.UserId == userId && t.ParentId == parentTopicId)
            .ToListAsync();
    }

    public async Task AddAsync(Topic topic)
    {
        await _context.Topics.InsertOneAsync(topic);
    }

    public async Task DeleteAsync(string id)
    {
        var topic = await GetByIdAsync(id);
        if (topic == null)
            return;

        // Buscar todo lo que empiece con la ruta de este tema
        var pathRegex = new BsonRegularExpression($"^{topic.Path}");

        // 1. Borramos el tema y todos sus subtemas
        var topicFilter = Builders<Topic>.Filter.Regex(t => t.Path, pathRegex);
        await _context.Topics.DeleteManyAsync(topicFilter);

        // 2. Borramos todas las notas que vivían en este tema o en sus subtemas
        var noteFilter = Builders<Note>.Filter.Regex(n => n.TopicPath, pathRegex);
        await _context.Notes.DeleteManyAsync(noteFilter);
    }
}
