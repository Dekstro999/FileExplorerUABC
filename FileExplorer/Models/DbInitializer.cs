using System;
using System.Linq;
using FileExplorer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FileExplorer.Data
{
    public static class DbInitializerV2 // Renamed to avoid CS0101 conflict  
    {
        public static void InitializeDatabase(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Folders.Any())
                {
                    return;
                }

                var uabc = new Folder { Name = "UABC" };
                context.Folders.Add(uabc);
                context.SaveChanges();

                var ensenada = new Folder { Name = "Ensenada", ParentId = uabc.Id };
                context.Folders.Add(ensenada);
                context.SaveChanges();

                var facultadArtes = new Folder { Name = "Facultad de Artes", ParentId = ensenada.Id };
                context.Folders.Add(facultadArtes);
                context.SaveChanges();

                context.Folders.AddRange(
                    new Folder { Name = "Artes Visuales", ParentId = facultadArtes.Id },
                    new Folder { Name = "Artes Musicales", ParentId = facultadArtes.Id },
                    new Folder { Name = "Artes Teatrales", ParentId = facultadArtes.Id },
                    new Folder { Name = "Artes Literarias", ParentId = facultadArtes.Id }
                );

                var facultadCiencias = new Folder { Name = "Facultad de Ciencias", ParentId = ensenada.Id };
                context.Folders.Add(facultadCiencias);
                context.SaveChanges();

                context.Folders.AddRange(
                    new Folder { Name = "Biologia", ParentId = facultadCiencias.Id },
                    new Folder { Name = "Matematicas", ParentId = facultadCiencias.Id },
                    new Folder { Name = "Fisica", ParentId = facultadCiencias.Id },
                    new Folder { Name = "Quimica", ParentId = facultadCiencias.Id }
                );

                var facultadCienciasMarinas = new Folder { Name = "Facultad de Ciencias Marinas", ParentId = ensenada.Id };
                context.Folders.Add(facultadCienciasMarinas);
                context.SaveChanges();

                context.Folders.AddRange(
                    new Folder { Name = "Biologia Marina", ParentId = facultadCienciasMarinas.Id },
                    new Folder { Name = "Oceanografia", ParentId = facultadCienciasMarinas.Id },
                    new Folder { Name = "Ciencias del Mar", ParentId = facultadCienciasMarinas.Id },
                    new Folder { Name = "Ingenieria en Ciencias del Mar", ParentId = facultadCienciasMarinas.Id },
                    new Folder { Name = "Ingenieria en Transporte Maritimo", ParentId = facultadCienciasMarinas.Id }
                );

                var facultadDeportes = new Folder { Name = "Facultad de Deportes", ParentId = ensenada.Id };
                context.Folders.Add(facultadDeportes);
                context.SaveChanges();

                context.Folders.AddRange(
                    new Folder { Name = "Ciencias del Deporte", ParentId = facultadDeportes.Id },
                    new Folder { Name = "Entrenamiento Deportivo", ParentId = facultadDeportes.Id },
                    new Folder { Name = "Rehabilitacion y Terapia Fisica", ParentId = facultadDeportes.Id },
                    new Folder { Name = "Nutricion y Dietetica", ParentId = facultadDeportes.Id },
                    new Folder { Name = "Educacion Fisica", ParentId = facultadDeportes.Id }
                );

                var facultadIngenieria = new Folder { Name = "Facultad de Ingenieria", ParentId = ensenada.Id };
                context.Folders.Add(facultadIngenieria);
                context.SaveChanges();

                context.Folders.AddRange(
                    new Folder { Name = "Tronco Comun de Ingenieria", ParentId = facultadIngenieria.Id },
                    new Folder { Name = "Ingenieria Civil", ParentId = facultadIngenieria.Id },
                    new Folder { Name = "Ingenieria en Electronica", ParentId = facultadIngenieria.Id },
                    new Folder { Name = "Ingenieria en Computacion", ParentId = facultadIngenieria.Id },
                    new Folder { Name = "Ingenieria Industrial", ParentId = facultadIngenieria.Id },
                    new Folder { Name = "Bioingenieria", ParentId = facultadIngenieria.Id }
                );

                context.SaveChanges();
            }
        }
    }
}
