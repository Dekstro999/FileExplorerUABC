using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileExplorer.Models;
using FileExplorer.Services;

namespace FileExplorer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderApiController : ControllerBase
    {
        private readonly FolderService _folderService;

        public FolderApiController(FolderService folderService)
        {
            _folderService = folderService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFolder(int id)
        {
            var folder = await _folderService.GetFolderByIdAsync(id);
            
            if (folder == null)
            {
                return NotFound();
            }
            
            return Ok(folder);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateFolder(NewFolderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var folder = await _folderService.CreateFolderAsync(model);
            
            return CreatedAtAction(nameof(GetFolder), new { id = folder.Id }, folder);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFolder(int id)
        {
            var result = await _folderService.DeleteFolderAsync(id);
            
            if (!result)
            {
                return BadRequest("No se puede eliminar la carpeta");
            }
            
            return NoContent();
        }
    }
}
