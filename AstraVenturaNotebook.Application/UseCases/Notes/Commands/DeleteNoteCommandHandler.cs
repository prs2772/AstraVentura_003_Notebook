using AstraVenturaNotebook.Application.Interfaces;
using AstraVenturaNotebook.Core.Interfaces;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Notes.Commands;

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, bool>
{
    private readonly INoteRepository _noteRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteNoteCommandHandler(
        INoteRepository noteRepository,
        ICurrentUserService currentUserService
    )
    {
        _noteRepository = noteRepository;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.GetByIdAsync(request.NoteId);

        if (note == null || note.UserId != _currentUserService.GetUserId())
        {
            return false;
        }

        await _noteRepository.DeleteAsync(request.NoteId);
        return true;
    }
}
