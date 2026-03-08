using AstraVenturaNotebook.Application.Dtos;
using AstraVenturaNotebook.Application.Interfaces;
using AstraVenturaNotebook.Core.Entities;
using AstraVenturaNotebook.Core.Interfaces;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Notes.Commands;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, NoteDto>
{
    private readonly ITopicRepository _topicRepository;
    private readonly INoteRepository _noteRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateNoteCommandHandler(
        ITopicRepository topicRepository,
        INoteRepository noteRepository,
        ICurrentUserService currentUserService
    )
    {
        _topicRepository = topicRepository;
        _noteRepository = noteRepository;
        _currentUserService = currentUserService;
    }

    public async Task<NoteDto> Handle(
        CreateNoteCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _currentUserService.GetUserId();

        // 1. Validar que el tema exista
        var topic = await _topicRepository.GetByIdAsync(request.TopicId);
        if (topic == null)
        {
            throw new Exception("Topic not found");
        }

        // 2. Validar que el tema le pertenezca al usuario del token
        if (topic.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                "You don't have permission to add notes to this topic."
            );
        }

        // 3. Crear la entidad de dominio usando el constructor de Core
        var noteId = Guid.NewGuid().ToString();
        var note = new Note(noteId, request.Title, request.Content, topic);

        // 4. Guardar en base de datos
        await _noteRepository.AddAsync(note);

        // 5. Retornar DTO
        return new NoteDto(note.Id, note.Title, note.Content, note.TopicAncestors);
    }
}
