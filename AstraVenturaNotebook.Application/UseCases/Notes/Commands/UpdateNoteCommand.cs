using AstraVenturaNotebook.Application.Dtos;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Notes.Commands;

public record UpdateNoteCommand(string NoteId, string Title, string Content) : IRequest<NoteDto>;
