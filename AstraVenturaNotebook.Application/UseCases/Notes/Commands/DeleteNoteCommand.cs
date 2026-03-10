using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Notes.Commands;

public record DeleteNoteCommand(string NoteId) : IRequest<bool>;
