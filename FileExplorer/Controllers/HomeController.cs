using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FileExplorer.Models;
using FileExplorer.Services;

namespace FileExplorer.Controllers
{
    public class HomeController : Controller
    {
        private readonly FileExplorerService _fileExplorerService;

        public HomeController(FileExplorerService fileExplorerService)
        {
            _fileExplorerService = fileExplorerService;
        }

        public async Task<IActionResult> Index()
        {
            var fileStructure = await _fileExplorerService.GetFileStructureAsync();
            return View(fileStructure);
        }

        [HttpGet]
        public async Task<IActionResult> GetFolderContents(string path)
        {
            var fileStructure = await _fileExplorerService.GetFileStructureAsync();

            FileNode targetNode = FindNodeByPath(fileStructure, path);

            if (targetNode != null)
            {
                return Json(targetNode.Children);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> FileTreePartial()
        {
            var fileStructure = await _fileExplorerService.GetFileStructureAsync();
            return PartialView("_FileTreePartial", fileStructure);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNode(string path)
        {
            // Lógica para encontrar el nodo por path y eliminarlo
            // Puedes usar tu servicio para esto
            var success = await _fileExplorerService.DeleteNodeByPathAsync(path);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddMateria(string Nombre, string SemestrePath)
        {
            var result = await _fileExplorerService.AddMateriaAsync(Nombre, SemestrePath);
            if (result) return Ok();
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddContenido(string Numero, string Titulo, string MateriaPath)
        {
            var result = await _fileExplorerService.AddContenidoAsync(Numero, Titulo, MateriaPath);
            if (result) return Ok();
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddRecursoContenido(string Nombre, string Url, int TipoArchivoId, string Descripcion, string ContenidoPath)
        {
            var result = await _fileExplorerService.AddRecursoContenidoAsync(Nombre, Url, TipoArchivoId, Descripcion, ContenidoPath);
            if (result) return Ok();
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRecursoContenido(int id)
        {
            var result = await _fileExplorerService.DeleteRecursoContenidoAsync(id);
            if (result) return Ok();
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetRecursosContenido(string path)
        {
            var recursos = await _fileExplorerService.GetRecursosContenidoAsync(path);
            if (recursos == null)
                return NotFound();
            return Json(recursos);
        }

        [HttpGet]
        public IActionResult GetTiposArchivo()
        {
            var tipos = _fileExplorerService.GetTiposArchivo();
            return Json(tipos.Select(t => new { id = t.Id, nombre = t.Nombre, extension = t.Extension }));
        }

        private FileNode FindNodeByPath(FileNode node, string path)
        {
            if (node.Path == path)
            {
                return node;
            }

            foreach (var child in node.Children)
            {
                var result = FindNodeByPath(child, path);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
