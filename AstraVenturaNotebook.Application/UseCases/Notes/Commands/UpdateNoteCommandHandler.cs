using AstraVenturaNotebook.Application.Dtos;
using AstraVenturaNotebook.Application.Interfaces;
using AstraVenturaNotebook.Core.Interfaces;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Notes.Commands;

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, NoteDto>
{
    private readonly INoteRepository _noteRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateNoteCommandHandler(
        INoteRepository noteRepository,
        ICurrentUserService currentUserService
    )
    {
        _noteRepository = noteRepository;
        _currentUserService = currentUserService;
    }

    public async Task<NoteDto> Handle(
        UpdateNoteCommand request,
        CancellationToken cancellationToken
    )
    {
        var note = await _noteRepository.GetByIdAsync(request.NoteId);

        if (note == null || note.UserId != _currentUserService.GetUserId())
        {
            throw new UnauthorizedAccessException("Note not found or access denied.");
        }

        note.UpdateContent(request.Title, request.Content);

        await _noteRepository.UpdateAsync(note);

        return new NoteDto(note.Id, note.Title, note.Content, note.TopicAncestors);
    }
}
