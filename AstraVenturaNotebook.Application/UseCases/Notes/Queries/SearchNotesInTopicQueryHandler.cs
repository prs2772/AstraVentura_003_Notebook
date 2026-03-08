using AstraVenturaNotebook.Application.Dtos;
using AstraVenturaNotebook.Application.Interfaces;
using AstraVenturaNotebook.Core.Interfaces;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Notes.Queries;

public class SearchNotesInTopicQueryHandler
    : IRequestHandler<SearchNotesInTopicQuery, IEnumerable<SearchResultDto>>
{
    private readonly ITopicRepository _topicRepository;
    private readonly INoteRepository _noteRepository;
    private readonly ICurrentUserService _currentUserService;

    public SearchNotesInTopicQueryHandler(
        ITopicRepository topicRepository,
        INoteRepository noteRepository,
        ICurrentUserService currentUserService
    )
    {
        _topicRepository = topicRepository;
        _noteRepository = noteRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<SearchResultDto>> Handle(
        SearchNotesInTopicQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _currentUserService.GetUserId();

        // 1. Obtener el tema base para saber su Path ("Ruta Materializada")
        var baseTopic = await _topicRepository.GetByIdAsync(request.TopicId);
        if (baseTopic == null || baseTopic.UserId != userId)
        {
            return Enumerable.Empty<SearchResultDto>();
        }

        // 2. Delegar la búsqueda pesada al repositorio, pasándole el Path
        // Por ejemplo, buscará en todo lo que empiece con "/topic_1/"
        var notes = await _noteRepository.SearchInPathAsync(
            userId,
            baseTopic.Path,
            request.SearchTerm
        );

        // 3. Mapear al DTO para el frontend
        return notes.Select(note => new SearchResultDto(
            NoteId: note.Id,
            Title: note.Title,
            // Extraer un fragmento (snippet) de los primeros 100 caracteres por ahora.
            Snippet: note.Content.Length > 100
                ? note.Content.Substring(0, 100) + "..."
                : note.Content,
            PathNames: note.TopicAncestors
        ));
    }
}
