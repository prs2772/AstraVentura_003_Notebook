using AstraVenturaNotebook.Application.Dtos;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Notes.Queries;

public record GetNotesByTopicIdQuery(string TopicId) : IRequest<IEnumerable<SearchResultDto>>;
