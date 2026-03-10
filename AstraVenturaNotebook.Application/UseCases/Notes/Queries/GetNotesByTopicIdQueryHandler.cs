using AstraVenturaNotebook.Application.Dtos;
using AstraVenturaNotebook.Application.Interfaces;
using AstraVenturaNotebook.Core.Interfaces;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Notes.Queries;

public class GetNotesByTopicIdQueryHandler
    : IRequestHandler<GetNotesByTopicIdQuery, IEnumerable<SearchResultDto>>
{
    private readonly ITopicRepository _topicRepository;
    private readonly INoteRepository _noteRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetNotesByTopicIdQueryHandler(
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
        GetNotesByTopicIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _currentUserService.GetUserId();

        // 1. Obtener el tema base para saber su Path
        var baseTopic = await _topicRepository.GetByIdAsync(request.TopicId);
        if (baseTopic == null || baseTopic.UserId != userId)
        {
            return Enumerable.Empty<SearchResultDto>();
        }

        // 2. Traer todas las notas debajo de ese topic
        var notes = await _noteRepository.GetByTopicPathAsync(userId, baseTopic.Path);

        // 3. Mapear al DTO (reusamos SearchResultDto para mantener la compatibilidad del UI)
        return notes.Select(note => new SearchResultDto(
            NoteId: note.Id,
            Title: note.Title,
            Snippet: note.Content,
            PathNames: note.TopicAncestors
        ));
    }
}
