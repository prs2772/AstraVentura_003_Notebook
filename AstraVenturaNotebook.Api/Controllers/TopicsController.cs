using AstraVenturaNotebook.Application.UseCases.Notes.Commands;
using AstraVenturaNotebook.Application.UseCases.Topics.Commands;
using AstraVenturaNotebook.Application.UseCases.Topics.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstraVenturaNotebook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TopicsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TopicsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetTopics([FromQuery] string? parentId)
    {
        // Si mandas un parentId, te da sus subtemas. Si no lo mandas, te da los temas raíz.
        var query = new GetTopicsQuery(parentId);
        var results = await _mediator.Send(query);

        return Ok(results);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTopic([FromBody] CreateTopicCommand command)
    {
        var result = await _mediator.Send(command);
        // Retornamos 201 Created junto con el DTO que incluye el ID generado
        return Created(string.Empty, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTopic(string id)
    {
        var result = await _mediator.Send(new DeleteTopicCommand(id));
        if (!result)
            return NotFound("Tema no encontrado.");

        return NoContent(); // 204 No Content, Eliminado exitosamente
    }
}
