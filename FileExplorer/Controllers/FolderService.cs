using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileExplorer.Data;
using FileExplorer.Models;
using FileExplorer.Services;
using Microsoft.EntityFrameworkCore;


namespace FileExplorer.Services
{
    public class FolderService
    {
        private readonly ApplicationDbContext _context;

        public FolderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Folder> GetRootFolderAsync()
        {
            // Obtener la carpeta raíz (UABC)
            var rootFolder = await _context.Folders
                .Include(f => f.Children)
                .FirstOrDefaultAsync(f => f.ParentId == null);

            if (rootFolder != null)
            {
                // Establecer la ruta y expandir la raíz
                rootFolder.Path = $"/{rootFolder.Name}";
                rootFolder.IsExpanded = true;

                // Cargar los hijos de primer nivel
                await LoadChildrenRecursivelyAsync(rootFolder, 2); // Cargar hasta 2 niveles
            }

            return rootFolder;
        }

        public async Task<Folder> GetFolderByIdAsync(int id)
        {
            var folder = await _context.Folders
                .Include(f => f.Children)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (folder != null)
            {
                // Construir la ruta completa
                folder.Path = await BuildPathAsync(folder);

                // Cargar los hijos directos
                foreach (var child in folder.Children)
                {
                    child.Path = $"{folder.Path}/{child.Name}";
                }
            }

            return folder;
        }

        public async Task<string> BuildPathAsync(Folder folder)
        {
            var path = $"/{folder.Name}";
            var current = folder;

            while (current.ParentId != null)
            {
                current = await _context.Folders.FindAsync(current.ParentId);
                if (current != null)
                {
                    path = $"/{current.Name}{path}";
                }
            }

            return path;
        }

        private async Task LoadChildrenRecursivelyAsync(Folder folder, int depth)
        {
            if (depth <= 0) return;

            // Cargar los hijos directos si aún no están cargados
            if (!folder.Children.Any())
            {
                var children = await _context.Folders
                    .Where(f => f.ParentId == folder.Id)
                    .ToListAsync();

                foreach (var child in children)
                {
                    child.Path = $"{folder.Path}/{child.Name}";
                    folder.Children.Add(child);
                }
            }

            // Cargar recursivamente los hijos de los hijos
            foreach (var child in folder.Children)
            {
                await LoadChildrenRecursivelyAsync(child, depth - 1);
            }
        }

        public async Task<Folder> CreateFolderAsync(NewFolderViewModel model)
        {
            var folder = new Folder
            {
                Name = model.Name,
                ParentId = model.ParentId
            };

            _context.Folders.Add(folder);
            await _context.SaveChangesAsync();

            // Cargar el padre para construir la ruta
            var parent = await GetFolderByIdAsync(model.ParentId);
            if (parent != null)
            {
                folder.Path = $"{parent.Path}/{folder.Name}";
            }
            else
            {
                folder.Path = $"/{folder.Name}";
            }

            return folder;
        }

        public async Task<bool> DeleteFolderAsync(int id)
        {
            var folder = await _context.Folders
                .Include(f => f.Children)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (folder == null) return false;

            // No permitir eliminar la carpeta raíz
            if (folder.ParentId == null) return false;

            // No permitir eliminar carpetas con hijos
            if (folder.Children.Any()) return false;

            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<int>> GetFolderPathIdsAsync(int folderId)
        {
            var pathIds = new List<int>();
            var currentFolder = await _context.Folders.FindAsync(folderId);

            while (currentFolder != null)
            {
                pathIds.Insert(0, currentFolder.Id);
                if (currentFolder.ParentId.HasValue)
                {
                    currentFolder = await _context.Folders.FindAsync(currentFolder.ParentId.Value);
                }
                else
                {
                    break;
                }
            }

            return pathIds;
        }
    }
}
