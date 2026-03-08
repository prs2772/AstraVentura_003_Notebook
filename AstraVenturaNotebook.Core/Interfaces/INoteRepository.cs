using AstraVenturaNotebook.Core.Entities;

namespace AstraVenturaNotebook.Core.Interfaces;

public interface INoteRepository
{
    Task<Note?> GetByIdAsync(string id);
    Task AddAsync(Note note);
    Task UpdateAsync(Note note);

    /// <summary>
    /// Busca texto dentro de una ruta específica (que incluye a todos sus hijos)
    /// </summary>
    /// <param name="userId">Id del usuario</param>
    /// <param name="basePath">Ruta base</param>
    /// <param name="searchTerm">Término de búsqueda</param>
    /// <returns>IEnumerable<Note> con las notas encontradas</returns>
    Task<IEnumerable<Note>> SearchInPathAsync(string userId, string basePath, string searchTerm);
}
