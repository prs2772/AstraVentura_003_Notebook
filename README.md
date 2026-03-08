<div align="center">
  <h1>📒 AstraVentura Notebook Microservice</h1>
  <p><i>Gestión de notas y cuadernos digitales jerárquicos para el ecosistema AstraVentura.</i></p>
  
  ![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)
  ![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white)
  ![MongoDB](https://img.shields.io/badge/MongoDB-4EA94B?style=flat-square&logo=mongodb&logoColor=white)
  ![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-blue?style=flat-square)
  ![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white)
</div>

# Astra Ventura Notebook

Repositorio GitHub: AstraVentura_003_Notebook

## 📌 Próposito

Crear una antología indexada de apuntes y propósitos. Funciona como una librería o cuaderno digital personal donde cada usuario puede guardar y estructurar su conocimiento por temas y subtemas de forma infinita.

## 🎯 Alcance

Dentro del ecosistema de Astra Ventura Universal, este microservicio actúa como el gestor de conocimiento documental. Se integra con AstraVenturaAuth para garantizar que cada antología sea estrictamente personal (validando los JWT emitidos). Destaca por su capacidad de realizar búsquedas recursivas de texto en cualquier nivel de la jerarquía de carpetas/temas de forma extremadamente rápida.

Se puede ver una explicación general del funcionamiento y la desnormalización en el archivo docs/explanation.md
→ [Explicación general](docs/explanation.md)

## 🏗 Arquitectura

Se usa **Clean Architecture** apoyada en el patrón **CQRS** (Command Query Responsibility Segregation) con MediatR.
→ [Arquitectura detallada](docs/architecture.md)

Para el manejo de la jerarquía infinita de temas y búsquedas recursivas rápidas, se utiliza MongoDB implementando el patrón de bases de datos documentales conocido como **Materialized Path** (Ruta Materializada).

Para revisar cómo se fue creando el proyecto y las decisiones técnicas iniciales, revisar el archivo project-creation.md
→ [Project Creation](docs/project-creation.md)

## 🚀 Ejecución

Ejecutar `docker compose up -d` para levantar la base de datos MongoDB y el gestor gráfico Mongo Express. Esto cargará lo necesario para el almacenamiento de los cuadernos. Para detener los servicios ejecutar `docker compose down`.

Para configurar las variables de entorno locales y la conexión segura con la autenticación (Secretos), revisar el archivo user-secrets.md.
→ [Configuración de Secretos](docs/user-secrets.md)

Para deployar en Azure revisar el archivo azure.md
→ [Azure](docs/azure.md)

## 🧪 Try me

Revisar el archivo try.md para ver la colección de endpoints (creación de temas, notas, borrado en cascada y búsqueda recursiva) y cómo probarlos en Insomnia/Postman.
→ [Try me](docs/try.md)

## 🛠️ Utilerías

Revisar utils.md para ver comandos útiles para el manejo de la base de datos documental y el microservicio.
→ [Utilerías](docs/utils.md)

## 📖 Documentation

- [Arquitectura detallada](docs/architecture.md)
- [Project Creation](docs/project-creation.md)
- [Configuración de Secretos (User Secrets)](docs/user-secrets.md)
- [Explicación general](docs/explanation.md)
- [Try me](docs/try.md)
