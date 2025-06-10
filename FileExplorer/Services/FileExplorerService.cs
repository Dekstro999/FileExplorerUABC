using Microsoft.EntityFrameworkCore;
using FileExplorer.Models;
using FileExplorer.Data;

public class FileExplorerService
{
    private readonly ApplicationDbContext _context;

    public FileExplorerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FileNode> GetFileStructureAsync()
    {
        var universidad = await _context.Universidades
            .Include(u => u.Campus)
                .ThenInclude(c => c.Facultades)
                    .ThenInclude(f => f.Carreras)
                        .ThenInclude(car => car.Semestres) // Incluir semestres
                            .ThenInclude(sem => sem.Materias) // Incluir materias
                                .ThenInclude(m => m.Contenidos) // Incluir contenidos
            .FirstOrDefaultAsync();

        if (universidad == null) return null;

        var root = new FileNode
        {
            Name = universidad.Nombre,
            Path = $"/{universidad.Nombre}",
            IsDirectory = true,
            IsExpanded = true,
            Children = universidad.Campus.Select(c => new FileNode
            {
                Name = c.Nombre,
                Path = $"/{universidad.Nombre}/{c.Nombre}",
                IsDirectory = true,
                Children = c.Facultades.Select(f => new FileNode
                {
                    Name = f.Nombre,
                    Path = $"/{universidad.Nombre}/{c.Nombre}/{f.Nombre}",
                    IsDirectory = true,
                    Children = f.Carreras.Select(car => new FileNode
                    {
                        Name = car.Nombre,
                        Path = $"/{universidad.Nombre}/{c.Nombre}/{f.Nombre}/{car.Nombre}",
                        IsDirectory = true,
                        Children = car.Semestres?.Select(sem => new FileNode
                        {
                            Name = sem.Nombre,
                            Path = $"/{universidad.Nombre}/{c.Nombre}/{f.Nombre}/{car.Nombre}/{sem.Nombre}",
                            IsDirectory = true,
                            Children = sem.Materias?.Select(m => 
                            {
                                var materiaNode = new FileNode
                                {
                                    Name = m.Nombre,
                                    Path = $"/{universidad.Nombre}/{c.Nombre}/{f.Nombre}/{car.Nombre}/{sem.Nombre}/{m.Nombre}",
                                    IsDirectory = m.Contenidos != null && m.Contenidos.Any(),
                                    Children = m.Contenidos?.Select(con => new FileNode
                                    {
                                        Name = $"{con.Numero} - {con.Titulo}",
                                        Path = $"/{universidad.Nombre}/{c.Nombre}/{f.Nombre}/{car.Nombre}/{sem.Nombre}/{m.Nombre}/{con.Numero}",
                                        IsDirectory = false
                                    }).ToList() ?? new List<FileNode>()
                                };
                                return materiaNode;
                            }).ToList() ?? new List<FileNode>()
                        }).ToList()

                    }).ToList()
                }).ToList()
            }).ToList()
        };

        return root;
    }

    public async Task<bool> DeleteNodeByPathAsync(string path)
    {
        // Ejemplo de path: /UABC/Ensenada/Facultad de Ingenieria/Ingenieria en Computacion/Semestre 3/Estructuras de Datos/1.1
        var parts = path.Trim('/').Split('/');

        if (parts.Length < 2)
            return false;

        // Eliminar Contenido
        if (parts.Length == 7)
        {
            var universidad = parts[0];
            var campus = parts[1];
            var facultad = parts[2];
            var carrera = parts[3];
            var semestre = parts[4];
            var materia = parts[5];
            var contenidoNumero = parts[6];

            var contenidoEntity = await _context.Contenidos
                .Include(c => c.Materia)
                    .ThenInclude(m => m.Semestre)
                        .ThenInclude(s => s.Carrera)
                            .ThenInclude(ca => ca.Facultad)
                                .ThenInclude(f => f.Campus)
                                    .ThenInclude(ca => ca.Universidad)
                .FirstOrDefaultAsync(c =>
                    c.Numero == contenidoNumero &&
                    c.Materia.Nombre == materia &&
                    c.Materia.Semestre.Nombre == semestre &&
                    c.Materia.Semestre.Carrera.Nombre == carrera &&
                    c.Materia.Semestre.Carrera.Facultad.Nombre == facultad &&
                    c.Materia.Semestre.Carrera.Facultad.Campus.Nombre == campus &&
                    c.Materia.Semestre.Carrera.Facultad.Campus.Universidad.Nombre == universidad
                );

            if (contenidoEntity != null)
            {
                _context.Contenidos.Remove(contenidoEntity);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        // Eliminar Materia
        if (parts.Length == 6)
        {
            var universidad = parts[0];
            var campus = parts[1];
            var facultad = parts[2];
            var carrera = parts[3];
            var semestre = parts[4];
            var materia = parts[5];

            var materiaEntity = await _context.Materias
                .Include(m => m.Semestre)
                    .ThenInclude(s => s.Carrera)
                        .ThenInclude(c => c.Facultad)
                            .ThenInclude(f => f.Campus)
                                .ThenInclude(ca => ca.Universidad)
                .FirstOrDefaultAsync(m =>
                    m.Nombre == materia &&
                    m.Semestre.Nombre == semestre &&
                    m.Semestre.Carrera.Nombre == carrera &&
                    m.Semestre.Carrera.Facultad.Nombre == facultad &&
                    m.Semestre.Carrera.Facultad.Campus.Nombre == campus &&
                    m.Semestre.Carrera.Facultad.Campus.Universidad.Nombre == universidad
                );

            if (materiaEntity != null)
            {
                _context.Materias.Remove(materiaEntity);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        // Eliminar Semestre (solo si no tiene materias)
        if (parts.Length == 5)
        {
            var universidad = parts[0];
            var campus = parts[1];
            var facultad = parts[2];
            var carrera = parts[3];
            var semestre = parts[4];

            var semestreEntity = await _context.Semestres
                .Include(s => s.Materias)
                .Include(s => s.Carrera)
                    .ThenInclude(c => c.Facultad)
                        .ThenInclude(f => f.Campus)
                            .ThenInclude(ca => ca.Universidad)
                .FirstOrDefaultAsync(s =>
                    s.Nombre == semestre &&
                    s.Carrera.Nombre == carrera &&
                    s.Carrera.Facultad.Nombre == facultad &&
                    s.Carrera.Facultad.Campus.Nombre == campus &&
                    s.Carrera.Facultad.Campus.Universidad.Nombre == universidad
                );

            if (semestreEntity != null && (semestreEntity.Materias == null || !semestreEntity.Materias.Any()))
            {
                _context.Semestres.Remove(semestreEntity);
                await _context.SaveChangesAsync();
                return true;
            }
            // Si tiene materias, no se puede borrar
            return false;
        }

        // 3. (Opcional) Implementar para Carrera, Facultad, etc.

        return false;
    }

    internal FileNode GetFileStructure()
    {
        throw new NotImplementedException();
    }

    public async Task<object> GetRecursoContenidoByIdAsync(int id)
    {
        var recurso = await _context.RecursosContenido
            .Include(r => r.TipoArchivo)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recurso == null) return null;

        return new
        {
            id = recurso.Id,
            nombre = recurso.Nombre,
            url = recurso.Url,
            tipoArchivoId = recurso.TipoArchivoId,
            descripcion = recurso.Descripcion
        };
    }

    public async Task<bool> AddMateriaAsync(string nombre, string semestrePath)
    {
        var parts = semestrePath.Trim('/').Split('/');
        if (parts.Length != 5)
            return false;

        var universidad = parts[0];
        var campus = parts[1];
        var facultad = parts[2];
        var carrera = parts[3];
        var semestre = parts[4];

        var semestreEntity = await _context.Semestres
            .Include(s => s.Carrera)
                .ThenInclude(c => c.Facultad)
                    .ThenInclude(f => f.Campus)
                        .ThenInclude(ca => ca.Universidad)
            .FirstOrDefaultAsync(s =>
                s.Nombre == semestre &&
                s.Carrera.Nombre == carrera &&
                s.Carrera.Facultad.Nombre == facultad &&
                s.Carrera.Facultad.Campus.Nombre == campus &&
                s.Carrera.Facultad.Campus.Universidad.Nombre == universidad
            );

        if (semestreEntity == null)
            return false;

        _context.Materias.Add(new Materia { Nombre = nombre, SemestreId = semestreEntity.Id });
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddContenidoAsync(string numero, string titulo, string materiaPath)
    {
        var parts = materiaPath.Trim('/').Split('/');
        if (parts.Length != 6)
            return false;

        var universidad = parts[0];
        var campus = parts[1];
        var facultad = parts[2];
        var carrera = parts[3];
        var semestre = parts[4];
        var materia = parts[5];

        var materiaEntity = await _context.Materias
            .Include(m => m.Semestre)
                .ThenInclude(s => s.Carrera)
                    .ThenInclude(c => c.Facultad)
                        .ThenInclude(f => f.Campus)
                            .ThenInclude(ca => ca.Universidad)
            .FirstOrDefaultAsync(m =>
                m.Nombre == materia &&
                m.Semestre.Nombre == semestre &&
                m.Semestre.Carrera.Nombre == carrera &&
                m.Semestre.Carrera.Facultad.Nombre == facultad &&
                m.Semestre.Carrera.Facultad.Campus.Nombre == campus &&
                m.Semestre.Carrera.Facultad.Campus.Universidad.Nombre == universidad
            );

        if (materiaEntity == null)
            return false;

        _context.Contenidos.Add(new Contenido { Numero = numero, Titulo = titulo, MateriaId = materiaEntity.Id });
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<object>> GetRecursosContenidoAsync(string path)
    {
        var parts = path.Trim('/').Split('/');
        if (parts.Length != 7)
            return null;

        var universidad = parts[0];
        var campus = parts[1];
        var facultad = parts[2];
        var carrera = parts[3];
        var semestre = parts[4];
        var materia = parts[5];
        var contenidoNumero = parts[6];

        var contenido = await _context.Contenidos
            .Include(c => c.RecursosContenido)
                .ThenInclude(r => r.TipoArchivo)
            .Include(c => c.Materia)
                .ThenInclude(m => m.Semestre)
                    .ThenInclude(s => s.Carrera)
                        .ThenInclude(ca => ca.Facultad)
                            .ThenInclude(f => f.Campus)
                                .ThenInclude(ca => ca.Universidad)
            .FirstOrDefaultAsync(c =>
                c.Numero == contenidoNumero &&
                c.Materia.Nombre == materia &&
                c.Materia.Semestre.Nombre == semestre &&
                c.Materia.Semestre.Carrera.Nombre == carrera &&
                c.Materia.Semestre.Carrera.Facultad.Nombre == facultad &&
                c.Materia.Semestre.Carrera.Facultad.Campus.Nombre == campus &&
                c.Materia.Semestre.Carrera.Facultad.Campus.Universidad.Nombre == universidad
            );

        if (contenido == null)
            return null;

        return contenido.RecursosContenido.Select(r => new {
            r.Id,
            r.Nombre,
            Tipo = r.TipoArchivo?.Nombre,
            r.Url,
            Descripcion = r.Descripcion,
            Fecha = r.FechaRegistro

        });
    }

    public IEnumerable<TipoArchivo> GetTiposArchivo()
    {
        return _context.TiposArchivo.ToList();
    }

    public async Task<bool> AddRecursoContenidoAsync(string nombre, string url, int tipoArchivoId, string descripcion, string contenidoPath)
    {
        var parts = contenidoPath.Trim('/').Split('/');
        if (parts.Length != 7)
            return false;

        var universidad = parts[0];
        var campus = parts[1];
        var facultad = parts[2];
        var carrera = parts[3];
        var semestre = parts[4];
        var materia = parts[5];
        var contenidoNumero = parts[6];

        var contenido = await _context.Contenidos
            .Include(c => c.Materia)
                .ThenInclude(m => m.Semestre)
                    .ThenInclude(s => s.Carrera)
                        .ThenInclude(ca => ca.Facultad)
                            .ThenInclude(f => f.Campus)
                                .ThenInclude(ca => ca.Universidad)
            .FirstOrDefaultAsync(c =>
                c.Numero == contenidoNumero &&
                c.Materia.Nombre == materia &&
                c.Materia.Semestre.Nombre == semestre &&
                c.Materia.Semestre.Carrera.Nombre == carrera &&
                c.Materia.Semestre.Carrera.Facultad.Nombre == facultad &&
                c.Materia.Semestre.Carrera.Facultad.Campus.Nombre == campus &&
                c.Materia.Semestre.Carrera.Facultad.Campus.Universidad.Nombre == universidad
            );

        if (contenido == null)
            return false;

        var recurso = new RecursoContenido
        {
            Nombre = nombre,
            Url = url,
            TipoArchivoId = tipoArchivoId,
            Descripcion = descripcion,
            ContenidoId = contenido.Id
        };
        _context.RecursosContenido.Add(recurso);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRecursoContenidoAsync(int id)
    {
        var recurso = await _context.RecursosContenido.FindAsync(id);
        if (recurso == null)
            return false;

        _context.RecursosContenido.Remove(recurso);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EditRecursoContenidoAsync(int id, string nombre, string url, int tipoArchivoId, string descripcion)
    {
        var recurso = await _context.RecursosContenido.FindAsync(id);
        if (recurso == null) return false;

        recurso.Nombre = nombre;
        recurso.Url = url;
        recurso.TipoArchivoId = tipoArchivoId;
        recurso.Descripcion = descripcion;
        await _context.SaveChangesAsync();
        return true;
    }
}