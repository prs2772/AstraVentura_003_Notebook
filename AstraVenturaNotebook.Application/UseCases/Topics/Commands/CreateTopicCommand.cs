using AstraVenturaNotebook.Application.Dtos;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Topics.Commands;

// El Comando: ParentId es opcional. Si es null, es un Tema Raíz (ej. Matemáticas)
public record CreateTopicCommand(string Name, string? ParentId) : IRequest<TopicResponseDto>;
