using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExplorer.Models
{
    public class Folder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty; // Fix for CS8618: Initialize with a default value

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Folder Parent { get; set; } = null!; // Fix for CS8618: Use null-forgiving operator

        public virtual ICollection<Folder> Children { get; set; }

        [NotMapped]
        public bool IsExpanded { get; set; }

        [NotMapped]
        public string Path { get; set; } = string.Empty; // Fix for CS8618: Initialize with a default value

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public Folder()
        {
            Children = new List<Folder>();
            IsExpanded = false;
            CreatedDate = DateTime.Now;
        }
    }
}
