using AstraVenturaNotebook.Application.Dtos;
using AstraVenturaNotebook.Application.Interfaces;
using AstraVenturaNotebook.Core.Interfaces;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Notes.Queries;

public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, NoteDto?>
{
    private readonly INoteRepository _noteRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetNoteByIdQueryHandler(
        INoteRepository noteRepository,
        ICurrentUserService currentUserService
    )
    {
        _noteRepository = noteRepository;
        _currentUserService = currentUserService;
    }

    public async Task<NoteDto?> Handle(
        GetNoteByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var note = await _noteRepository.GetByIdAsync(request.NoteId);

        if (note == null || note.UserId != _currentUserService.GetUserId())
        {
            return null;
        }

        return new NoteDto(note.Id, note.Title, note.Content, note.TopicAncestors);
    }
}
