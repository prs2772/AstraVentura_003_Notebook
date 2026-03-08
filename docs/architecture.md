# AstraVenturaNotebook Architecture

Microservicio encargado de la gestión de notas y cuadernos digitales jerárquicos (estilo antología) para el ecosistema AstraVentura. 
Construido con .NET Core, Clean Architecture y MongoDB.

## Project Structure

El proyecto está dividido en 4 capas principales siguiendo la regla de dependencia (de afuera hacia adentro):

📦 AstraVenturaNotebook
 ┣ 📂 src
 ┃ ┣ 📂 1. AstraVenturaNotebook.Core           # (Dominio)
 ┃ ┃ ┣ 📂 Entities                             # Topic, Note (Modelos con Materialized Path)
 ┃ ┃ ┣ 📂 Exceptions                           # Excepciones de dominio (ej. TopicNotFoundException)
 ┃ ┃ ┗ 📂 Interfaces                           # ITopicRepository, INoteRepository
 ┃ ┃
 ┃ ┣ 📂 2. AstraVenturaNotebook.Application    # (Casos de Uso)
 ┃ ┃ ┣ 📂 DTOs                                 # SearchResultDto, TopicDto, NoteDto
 ┃ ┃ ┣ 📂 UseCases / Features                  # CreateTopicCommand, SearchNotesQuery
 ┃ ┃ ┗ 📂 Interfaces                           # ITokenService (Para validar el token de AstraVenturaAuth)
 ┃ ┃
 ┃ ┣ 📂 3. AstraVenturaNotebook.Infrastructure # (Implementación técnica)
 ┃ ┃ ┣ 📂 Persistence                          # MongoDbTopicRepository, MongoDbContext
 ┃ ┃ ┗ 📂 ExternalServices                     # Implementación de llamadas HTTP a AstraVenturaAuth (si aplica)
 ┃ ┃
 ┃ ┗ 📂 4. AstraVenturaNotebook.API            # (Presentación)
 ┃   ┣ 📂 Controllers                          # TopicsController, NotesController
 ┃   ┣ 📂 Middleware                           # Manejo global de errores, validación JWT
 ┃   ┗ 📜 Program.cs                           # Inyección de dependencias y configuración
 ┃
 ┗ 📜 compose.yml                              # Orquestación de contenedores locales
