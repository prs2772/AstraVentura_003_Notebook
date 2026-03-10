using AstraVenturaNotebook.Application.UseCases.Notes.Commands;
using AstraVenturaNotebook.Application.UseCases.Notes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstraVenturaNotebook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteCommand command)
    {
        // El controlador solo delega la responsabilidad a MediatR
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchNotes(
        [FromQuery] string topicId,
        [FromQuery] string? searchTerm
    )
    {
        if (string.IsNullOrWhiteSpace(topicId))
        {
            return BadRequest("topicId is required.");
        }

        var query = new SearchNotesInTopicQuery(topicId, searchTerm ?? string.Empty);
        var results = await _mediator.Send(query);

        return Ok(results);
    }

    [HttpGet("topic/{topicId}")]
    public async Task<IActionResult> GetNotesByTopic(string topicId)
    {
        if (string.IsNullOrWhiteSpace(topicId))
        {
            return BadRequest("topicId is required.");
        }

        var query = new GetNotesByTopicIdQuery(topicId);
        var results = await _mediator.Send(query);

        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoteById(string id)
    {
        var note = await _mediator.Send(new GetNoteByIdQuery(id));
        if (note == null)
            return NotFound();

        return Ok(note);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(string id, [FromBody] UpdateNoteCommand command)
    {
        if (id != command.NoteId)
            return BadRequest("Note ID does not match.");

        try
        {
            var updatedNote = await _mediator.Send(command);
            return Ok(updatedNote);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(string id)
    {
        var result = await _mediator.Send(new DeleteNoteCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }
}
