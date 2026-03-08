using System.Collections.Generic;
using AstraVenturaNotebook.Core.Entities;

namespace AstraVenturaNotebook.Application.Dtos;

public record TopicResponseDto(string Id, string Name, string Path, List<string> DnaChain);
