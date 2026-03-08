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
        [FromQuery] string searchTerm
    )
    {
        if (string.IsNullOrWhiteSpace(topicId) || string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("topicId and searchTerm are required.");
        }

        var query = new SearchNotesInTopicQuery(topicId, searchTerm);
        var results = await _mediator.Send(query);

        return Ok(results);
    }
}
