using Microsoft.EntityFrameworkCore;
using FileExplorer.Models;

namespace FileExplorer.Data
{
    public class ContextoBD : DbContext
    {
        public ContextoBD(DbContextOptions<ContextoBD> options) : base(options)
        {
        }

        // Definir DbSets para las entidades
        public DbSet<FileNode> FileNodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de entidades
            modelBuilder.Entity<FileNode>(entity =>
            {
                entity.HasKey(e => e.Path);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.IsDirectory).IsRequired();
                entity.Property(e => e.IsExpanded).IsRequired();

                // Ignorar la propiedad Children ya que no se almacena en la base de datos
                entity.Ignore(e => e.Children);
            });

            // Datos iniciales
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Datos iniciales para la estructura de carpetas UABC
            modelBuilder.Entity<FileNode>().HasData(
                new FileNode { Name = "UABC", Path = "/UABC", IsDirectory = true, IsExpanded = true },
                new FileNode { Name = "Ensenada", Path = "/UABC/Ensenada", IsDirectory = true, IsExpanded = true },

                // Facultad de Artes
                new FileNode { Name = "Facultad de Artes", Path = "/UABC/Ensenada/Facultad de Artes", IsDirectory = true },
                new FileNode { Name = "Artes Visuales", Path = "/UABC/Ensenada/Facultad de Artes/Artes Visuales", IsDirectory = true },
                new FileNode { Name = "Artes Musicales", Path = "/UABC/Ensenada/Facultad de Artes/Artes Musicales", IsDirectory = true },
                new FileNode { Name = "Artes Teatrales", Path = "/UABC/Ensenada/Facultad de Artes/Artes Teatrales", IsDirectory = true },
                new FileNode { Name = "Artes Literarias", Path = "/UABC/Ensenada/Facultad de Artes/Artes Literarias", IsDirectory = true },

                // Facultad de Ciencias
                new FileNode { Name = "Facultad de Ciencias", Path = "/UABC/Ensenada/Facultad de Ciencias", IsDirectory = true },
                new FileNode { Name = "Biologia", Path = "/UABC/Ensenada/Facultad de Ciencias/Biologia", IsDirectory = true },
                new FileNode { Name = "Matematicas", Path = "/UABC/Ensenada/Facultad de Ciencias/Matematicas", IsDirectory = true },
                new FileNode { Name = "Fisica", Path = "/UABC/Ensenada/Facultad de Ciencias/Fisica", IsDirectory = true },
                new FileNode { Name = "Quimica", Path = "/UABC/Ensenada/Facultad de Ciencias/Quimica", IsDirectory = true },

                // Facultad de Ciencias Marinas
                new FileNode { Name = "Facultad de Ciencias Marinas", Path = "/UABC/Ensenada/Facultad de Ciencias Marinas", IsDirectory = true },
                new FileNode { Name = "Biologia Marina", Path = "/UABC/Ensenada/Facultad de Ciencias Marinas/Biologia Marina", IsDirectory = true },
                new FileNode { Name = "Oceanografia", Path = "/UABC/Ensenada/Facultad de Ciencias Marinas/Oceanografia", IsDirectory = true },
                new FileNode { Name = "Ciencias del Mar", Path = "/UABC/Ensenada/Facultad de Ciencias Marinas/Ciencias del Mar", IsDirectory = true },
                new FileNode { Name = "Ingenieria en Ciencias del Mar", Path = "/UABC/Ensenada/Facultad de Ciencias Marinas/Ingenieria en Ciencias del Mar", IsDirectory = true },
                new FileNode { Name = "Ingenieria en Transporte Maritimo", Path = "/UABC/Ensenada/Facultad de Ciencias Marinas/Ingenieria en Transporte Maritimo", IsDirectory = true },

                // Facultad de Deportes
                new FileNode { Name = "Facultad de Deportes", Path = "/UABC/Ensenada/Facultad de Deportes", IsDirectory = true },
                new FileNode { Name = "Ciencias del Deporte", Path = "/UABC/Ensenada/Facultad de Deportes/Ciencias del Deporte", IsDirectory = true },
                new FileNode { Name = "Entrenamiento Deportivo", Path = "/UABC/Ensenada/Facultad de Deportes/Entrenamiento Deportivo", IsDirectory = true },
                new FileNode { Name = "Rehabilitacion y Terapia Fisica", Path = "/UABC/Ensenada/Facultad de Deportes/Rehabilitacion y Terapia Fisica", IsDirectory = true },
                new FileNode { Name = "Nutricion y Dietetica", Path = "/UABC/Ensenada/Facultad de Deportes/Nutricion y Dietetica", IsDirectory = true },
                new FileNode { Name = "Educacion Fisica", Path = "/UABC/Ensenada/Facultad de Deportes/Educacion Fisica", IsDirectory = true },

                // Facultad de Ingenieria
                new FileNode { Name = "Facultad de Ingenieria", Path = "/UABC/Ensenada/Facultad de Ingenieria", IsDirectory = true },
                new FileNode { Name = "Tronco Comun de Ingenieria", Path = "/UABC/Ensenada/Facultad de Ingenieria/Tronco Comun de Ingenieria", IsDirectory = true },
                new FileNode { Name = "Ingenieria Civil", Path = "/UABC/Ensenada/Facultad de Ingenieria/Ingenieria Civil", IsDirectory = true },
                new FileNode { Name = "Ingenieria en Electronica", Path = "/UABC/Ensenada/Facultad de Ingenieria/Ingenieria en Electronica", IsDirectory = true },
                new FileNode { Name = "Ingenieria en Computacion", Path = "/UABC/Ensenada/Facultad de Ingenieria/Ingenieria en Computacion", IsDirectory = true },
                new FileNode { Name = "Ingenieria Industrial", Path = "/UABC/Ensenada/Facultad de Ingenieria/Ingenieria Industrial", IsDirectory = true },
                new FileNode { Name = "Bioingenieria", Path = "/UABC/Ensenada/Facultad de Ingenieria/Bioingenieria", IsDirectory = true }
            );
        }
    }
}
