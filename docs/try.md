# 🧪 Try Me - AstraVentura Notebook API

Esta guía detalla cómo probar los endpoints principales del microservicio utilizando herramientas como Insomnia o Postman.

## 🔐 Prerrequisito: Autenticación
Todos los endpoints están protegidos. Necesitas un token JWT generado por `AstraVenturaAuth`. (Puedes bajar el proyecto en https://github.com/prs2772/AstraVentura_001_Auth)
* En Insomnia/Postman, ve a la de headers y agrega **Authorization**.
* Vas a escribir **Bearer [el jwt generado por AstraVenturaAuth, sin los corchetes]**.
* Pega tu JWT válido (tiene una duración corta).

---

## 📁 1. Gestión de Temas (Topics)

### 1.1 Crear un Tema Raíz (Ej. Matemáticas)
* **Método:** `POST`
* **URL:** `http://localhost:5010/api/topics`
* **Body (JSON):**
```json
{
  "name": "Matemáticas",
  "parentId": null
}
```

(Guarda el id que te devuelve la respuesta para el siguiente paso).

### 1.2 Crear un Subtema (Ej. Álgebra)
* **Método:** `POST`
* **URL:** `http://localhost:5010/api/topics`
* **Body (JSON):**
```json
{
  "name": "Álgebra",
  "parentId": "ID_DEL_TEMA_RAIZ"
}
```

(Guarda el id que te devuelve la respuesta para el siguiente paso).

### 1.3 Listar Temas
* **Método:** `GET`
* **URL (Raíces):** `http://localhost:5010/api/topics`
* **URL (Subtemas):** `http://localhost:5010/api/topics?parentId=ID_DEL_TEMA_PADRE`

## 📝 2. Gestión de Notas (Notes)

### 2.1 Crear una Nota
* **Método:** `POST`
* **URL:** `http://localhost:5010/api/notes`
* **Body (JSON):**
```json
{
  "topicId": "ID_DEL_SUBTEMA_ALGEBRA",
  "title": "Bases del álgebra",
  "content": "El álgebra es la rama de la matemática... Una de sus propiedades principales es..."
}
```

(Guarda el id devuelto o el noteId).

### 2.2 Actualizar una Nota
* **Método:** `PUT`
* **URL:** `http://localhost:5010/api/notes/ID_DE_LA_NOTA`
* **Body (JSON):**
```json
{
  "noteId": "ID_DE_LA_NOTA",
  "title": "Bases del álgebra (Actualizado)",
  "content": "Contenido actualizado con más información vital."
}
```

## 🔍 3. Búsqueda y Destrucción (Core Features)

### 3.1 Búsqueda Recursiva

Busca texto dentro de un tema y todos sus subtemas anidados de forma automática.

* **Método:** `GET`
* **URL:** `http://localhost:5010/api/notes/search?topicId=ID_DEL_TEMA_MATEMATICAS&searchTerm=propiedades`

(Enviar el ID del tema padre o raíz para ver cómo encuentra la nota guardada en el subtema).

### 3.2 Borrado en Cascada (Orden 66)
Elimina un tema, todos sus subtemas infinitos y todas las notas contenidas en esa rama.

* **Método:** `DELETE`
* **URL:** `http://localhost:5010/api/topics/ID_DEL_TEMA_MATEMATICAS
(Devuelve un 204 No Content si fue exitoso)`
