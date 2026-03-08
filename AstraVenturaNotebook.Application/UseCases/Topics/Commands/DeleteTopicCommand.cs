using AstraVenturaNotebook.Application.Interfaces;
using AstraVenturaNotebook.Core.Interfaces;
using MediatR;

namespace AstraVenturaNotebook.Application.UseCases.Topics.Commands;

public record DeleteTopicCommand(string TopicId) : IRequest<bool>;
