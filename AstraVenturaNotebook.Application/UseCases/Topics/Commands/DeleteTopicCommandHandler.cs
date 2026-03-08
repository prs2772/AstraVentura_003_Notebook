using AstraVenturaNotebook.Application.Interfaces;
using AstraVenturaNotebook.Core.Interfaces;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Topics.Commands;

public class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, bool>
{
    private readonly ITopicRepository _topicRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTopicCommandHandler(
        ITopicRepository topicRepository,
        ICurrentUserService currentUserService
    )
    {
        _topicRepository = topicRepository;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        var topic = await _topicRepository.GetByIdAsync(request.TopicId);

        if (topic == null)
            return false;

        // Seguridad: Solo el dueño puede borrarlo
        if (topic.UserId != userId)
            throw new UnauthorizedAccessException("No tienes permiso para borrar este tema.");

        await _topicRepository.DeleteAsync(topic.Id);
        return true;
    }
}
