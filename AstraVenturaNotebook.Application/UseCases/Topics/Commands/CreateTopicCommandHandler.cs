using AstraVenturaNotebook.Application.Dtos;
using AstraVenturaNotebook.Application.Interfaces;
using AstraVenturaNotebook.Core.Entities;
using AstraVenturaNotebook.Core.Interfaces;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Topics.Commands;

public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, TopicResponseDto>
{
    private readonly ITopicRepository _topicRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateTopicCommandHandler(
        ITopicRepository topicRepository,
        ICurrentUserService currentUserService
    )
    {
        _topicRepository = topicRepository;
        _currentUserService = currentUserService;
    }

    public async Task<TopicResponseDto> Handle(
        CreateTopicCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _currentUserService.GetUserId();
        var topicId = Guid.NewGuid().ToString();
        Topic newTopic;

        if (string.IsNullOrWhiteSpace(request.ParentId))
        {
            // Es un tema raíz
            newTopic = new Topic(topicId, userId, request.Name);
        }
        else
        {
            // Es un subtema, buscamos al padre
            var parentTopic = await _topicRepository.GetByIdAsync(request.ParentId);

            if (parentTopic == null)
                throw new Exception("Parent topic not found");

            if (parentTopic.UserId != userId)
                throw new UnauthorizedAccessException("You don't own the parent topic.");

            newTopic = new Topic(topicId, request.Name, parentTopic);
        }

        await _topicRepository.AddAsync(newTopic);

        return new TopicResponseDto(newTopic.Id, newTopic.Name, newTopic.Path, newTopic.DnaChain);
    }
}
