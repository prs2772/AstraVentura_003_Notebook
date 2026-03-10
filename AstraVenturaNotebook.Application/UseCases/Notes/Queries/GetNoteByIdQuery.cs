using AstraVenturaNotebook.Application.Dtos;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Notes.Queries;

public record GetNoteByIdQuery(string NoteId) : IRequest<NoteDto?>;
