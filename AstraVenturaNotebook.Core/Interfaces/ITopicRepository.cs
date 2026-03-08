using AstraVenturaNotebook.Core.Entities;

namespace AstraVenturaNotebook.Core.Interfaces;

public interface ITopicRepository
{
    Task<Topic?> GetByIdAsync(string id);
    Task<IEnumerable<Topic>> GetSubtopicsAsync(string userId, string? parentTopicId);
    Task AddAsync(Topic topic);
    Task DeleteAsync(string id); // Deberá borrar recursivamente basado en el Path
}
