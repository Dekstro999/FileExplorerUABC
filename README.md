
# FileExplorer UABC

Explorador de archivos tipo árbol para la estructura académica de la UABC, desarrollado con **ASP.NET Core Razor Pages (.NET 8)**, **Entity Framework Core** y **SQL Server**.

---

## Características

- Visualización jerárquica de carpetas y entidades académicas (Universidad, Campus, Facultad, Carrera, Semestre, Materia, Contenido, Recursos).
- Navegación interactiva y dinámica.
- CRUD de materias, contenidos y recursos.
- Persistencia en base de datos SQL Server.
- Código desacoplado y limpio (servicios, controladores, modelos).
- Bootstrap para UI responsiva.

---

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 o superior
- **SQL Server** (Express, LocalDB o instancia propia)

---

## Paquetes NuGet utilizados

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`

---

## Configuración de la base de datos

1. **Crea la base de datos y las tablas**  
   Ejecuta el script SQL que se encuentra en el archivo [`SqlServer`](FileExplorer/SqlServer) sobre tu instancia de SQL Server.  
   Esto creará la estructura necesaria para el proyecto.

2. **Configura la cadena de conexión**  
   Edita el archivo `appsettings.json` y coloca el nombre de tu servidor SQL Server y la base de datos creada.  
   Ejemplo:

   ```bash
   dotnet ef database update

---

## Ejecución

1. Abre el proyecto en Visual Studio 2022.
2. Asegúrate de que la cadena de conexión en `appsettings.json` es correcta.
3. Ejecuta el proyecto (`F5` o `Ctrl+F5`).
4. El sistema inicializará la estructura de carpetas y entidades si la base de datos está vacía.

---

## Estructura de la base de datos

* **Universidad** → **Campus** → **Facultad** → **Carrera** → **Semestre** → **Materia** → **Contenido** → **RecursoContenido**
* Tablas relacionadas con claves foráneas y restricciones de integridad.

---

## Personalización

* Modifica la estructura inicial en `DbInitializer.cs` si deseas cambiar la jerarquía o agregar datos semilla.
* Para agregar nuevas funcionalidades, revisa los controladores y servicios.

---

## Notas

* El proyecto está preparado para crecer y soportar archivos, permisos y otras entidades.
* Si tienes problemas con las migraciones o la base de datos, revisa la cadena de conexión y ejecuta el script SQL manualmente.

---

## Ejemplo de cadena de conexión

```json
{
  "ConnectionStrings": {
    "ConexionBD": "Server=DESKTOP-12345\\SQLEXPRESS;Database=FileExplorerDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```



