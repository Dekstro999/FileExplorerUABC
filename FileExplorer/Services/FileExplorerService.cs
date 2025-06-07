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

    internal FileNode GetFileStructure()
    {
        throw new NotImplementedException();
    }
}