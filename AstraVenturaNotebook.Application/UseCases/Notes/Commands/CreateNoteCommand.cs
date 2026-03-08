using AstraVenturaNotebook.Application.Dtos;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Notes.Commands;

public record CreateNoteCommand(string TopicId, string Title, string Content) : IRequest<NoteDto>;
