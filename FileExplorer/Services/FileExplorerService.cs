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
                            Children = sem.Materias?.Select(m => new FileNode
                            {
                                Name = m.Nombre,
                                Path = $"/{universidad.Nombre}/{c.Nombre}/{f.Nombre}/{car.Nombre}/{sem.Nombre}/{m.Nombre}",
                                IsDirectory = false
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
        // Ejemplo de path: /UABC/Ensenada/Facultad de Ingenieria/Ingenieria en Computacion/Semestre 3/Estructuras de Datos
        var parts = path.Trim('/').Split('/');

        if (parts.Length < 2)
            return false;

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
}