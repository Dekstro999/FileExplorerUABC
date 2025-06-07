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
