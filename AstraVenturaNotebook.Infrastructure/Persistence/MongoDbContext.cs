using AstraVenturaNotebook.Core.Entities;
using MongoDB.Driver;

namespace AstraVenturaNotebook.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Topic> Topics => _database.GetCollection<Topic>("Topics");
    public IMongoCollection<Note> Notes => _database.GetCollection<Note>("Notes");
}
