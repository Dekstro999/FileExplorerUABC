# FileExplorer UABC

Explorador de archivos tipo �rbol para la estructura de carpetas de la UABC, desarrollado con ASP.NET Core Razor Pages (.NET 8), Entity Framework Core y Bootstrap.

## Caracter�sticas

- Visualizaci�n jer�rquica de carpetas (tipo �rbol) de la UABC.
- Navegaci�n interactiva con AJAX (sin recargar la p�gina).
- Breadcrumb din�mico para mostrar la ruta actual.
- Estructura de datos persistente en base de datos (SQLite por defecto).
- C�digo limpio y desacoplado (servicios, controladores, modelos).

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 o superior
- SQLite (por defecto) o SQL Server (opcional)

## Paqueter�as NuGet utilizadas

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Sqlite`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation`
- `Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore`
- `Microsoft.Extensions.DependencyInjection`
- `Microsoft.AspNetCore.Mvc.NewtonsoftJson` (si se requiere serializaci�n avanzada)


Desde Visual Studio, presiona F5.

## Estructura de la base de datos

- **Modelo principal:** `FileNode`
  - `Path` (clave primaria, string)
  - `Name` (nombre de la carpeta)
  - `IsDirectory` (bool)
  - `IsExpanded` (bool)
  - `Children` (propiedad ignorada en la BD, solo para estructura en memoria)

- **Inicializaci�n:**  
  La estructura de carpetas se inicializa autom�ticamente al crear la base de datos, usando datos semilla en `ContextoBD.cs`.

## �C�mo funciona?

- El �rbol de carpetas se renderiza en la vista principal usando Razor y Bootstrap.
- Al hacer clic en una carpeta, se realiza una petici�n AJAX para cargar su contenido sin recargar la p�gina.
- El backend responde con los hijos de la carpeta seleccionada, consultando la base de datos.
- El breadcrumb se actualiza din�micamente para reflejar la ruta actual.

## Personalizaci�n

- Puedes modificar la estructura inicial de carpetas editando el m�todo `SeedData` en `ContextoBD.cs`.
- Para agregar nuevas funcionalidades (crear, eliminar carpetas), revisa los controladores y servicios (`FolderApiController`, `FolderService`).

## Notas

- El proyecto est� preparado para crecer y soportar archivos, permisos, y otras entidades si lo requieres.
- Si tienes problemas con las migraciones, ejecuta: