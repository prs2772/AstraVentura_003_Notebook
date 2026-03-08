using AstraVenturaNotebook.Application.Dtos;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Notes.Queries;

public record SearchNotesInTopicQuery(string TopicId, string SearchTerm)
    : IRequest<IEnumerable<SearchResultDto>>;
