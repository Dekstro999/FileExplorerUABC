using System.ComponentModel.DataAnnotations;

namespace FileExplorer.Models
{
    public class NewFolderViewModel
    {
        [Required(ErrorMessage = "El nombre de la carpeta es obligatorio")]
        [StringLength(255, ErrorMessage = "El nombre no puede exceder los 255 caracteres")]
        [RegularExpression(@"^[^<>:""/\\|?*]+$", ErrorMessage = "El nombre contiene caracteres no válidos")]
        public string Name { get; set; }

        [Required]
        public int ParentId { get; set; }

        public string ParentName { get; set; }
    }
}
