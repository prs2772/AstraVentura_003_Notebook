using AstraVenturaNotebook.Application.Interfaces;
using AstraVenturaNotebook.Core.Interfaces;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Topics.Queries;

public record TopicDto(string Id, string Name, string Path, string? ParentId);

public record GetTopicsQuery(string? ParentId) : IRequest<IEnumerable<TopicDto>>;

public class GetTopicsQueryHandler : IRequestHandler<GetTopicsQuery, IEnumerable<TopicDto>>
{
    private readonly ITopicRepository _topicRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetTopicsQueryHandler(
        ITopicRepository topicRepository,
        ICurrentUserService currentUserService
    )
    {
        _topicRepository = topicRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<TopicDto>> Handle(
        GetTopicsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _currentUserService.GetUserId();
        var topics = await _topicRepository.GetSubtopicsAsync(userId, request.ParentId);

        return topics.Select(t => new TopicDto(t.Id, t.Name, t.Path, t.ParentId));
    }
}
