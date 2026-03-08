# 🧠 Explicación General y Decisiones Arquitectónicas

El microservicio **AstraVentura Notebook** gestiona estructuras de conocimiento jerárquico. Para lograr un rendimiento óptimo en la lectura y búsqueda, abandona los enfoques relacionales tradicionales en favor de patrones específicos de bases de datos documentales (NoSQL - MongoDB).

## 1. El Problema de las Jerarquías en SQL

En una base de datos relacional estricta, la jerarquía se maneja comúnmente con el patrón *Adjacency List* (un registro apunta a su `ParentId`). 
Si se necesita buscar una palabra dentro del cuaderno "Matemáticas" y todos sus subtemas, el motor SQL debe:
1. Usar *CTEs Recursivas* (Common Table Expressions) para recorrer el árbol y obtener todos los IDs hijos.
2. Hacer un `JOIN` con la tabla de Notas.
3. Ejecutar la búsqueda de texto completo.
Este proceso es costoso computacionalmente (CPU y memoria) a medida que el árbol crece.

## 2. La Solución: Patrón Materialized Path (Ruta Materializada)

En lugar de que un Tema solo conozca a su padre directo, cada Tema construye y almacena su ruta completa desde la raíz.

**Ejemplo:**
* Matemáticas -> `Path: /topic_1/`
* Álgebra -> `Path: /topic_1/topic_3/`

Esto permite consultar ramas enteras utilizando expresiones regulares ancladas al inicio (`^/topic_1/`), lo cual permite a MongoDB utilizar índices de forma tan eficiente como si fuera una búsqueda de igualdad exacta.

## 3. Desnormalización Estratégica en Notas

La **desnormalización** consiste en duplicar datos intencionalmente para evitar *joins* (o *lookups* en MongoDB) en tiempo de lectura.

En este dominio, la entidad `Note` no solo guarda su `TopicId`. En el momento de su creación, copia de su Tema padre:
1. **El `TopicPath`**: Ej. `/topic_1/topic_3/`
2. **Los `TopicAncestors`**: Ej. `["Matemáticas", "Álgebra"]`

### ¿Por que usar la desnormalización estratégica?

* **Búsqueda en O(1) viaje a BD:** Para buscar recursivamente, la API consulta directamente la colección `Notes` filtrando por el `TopicPath` y la cadena de texto en un solo paso.

* **Breadcrumbs instantáneos:** Al devolver los resultados, la API no necesita consultar la colección `Topics` para saber cómo se llaman las carpetas padre; la nota ya contiene su "ADN" o lista de ancestros para que el frontend pinte la ruta de navegación inmediatamente.

### Trade-off (Costo)

La escritura se vuelve más compleja. Si un tema cambia de nombre o de ubicación en el árbol, el sistema debe actualizar iterativamente las rutas materializadas de todos sus hijos y las notas asociadas. Dado que un cuaderno digital tiene un ratio de Lectura/Escritura del 99% a 1% respecto al movimiento masivo de carpetas, este *trade-off* es altamente ventajoso para el ecosistema.
