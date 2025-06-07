using System.Collections.Generic;

namespace FileExplorer.Models
{
    public class FileNode
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsDirectory { get; set; }
        public bool IsExpanded { get; set; }
        public List<FileNode> Children { get; set; }

        public FileNode()
        {
            Children = new List<FileNode>();
            IsExpanded = false;
        }
    }
}
