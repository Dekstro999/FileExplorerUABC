using System.Collections.Generic;
using FileExplorer.Models;

namespace FileExplorer.Services
{
    public class FileExplorerService
    {
        public FileNode GetFileStructure()
        {
            // Crear la estructura de archivos de UABC
            var root = new FileNode
            {
                Name = "UABC",
                Path = "/UABC",
                IsDirectory = true,
                IsExpanded = true
            };

            var ensenada = new FileNode
            {
                Name = "Ensenada",
                Path = "/UABC/Ensenada",
                IsDirectory = true,
                IsExpanded = true
            };
            root.Children.Add(ensenada);

            // Facultad de Artes
            var facultadArtes = new FileNode
            {
                Name = "Facultad de Artes",
                Path = "/UABC/Ensenada/Facultad de Artes",
                IsDirectory = true
            };
            ensenada.Children.Add(facultadArtes);

            facultadArtes.Children.Add(new FileNode
            {
                Name = "Artes Visuales",
                Path = "/UABC/Ensenada/Facultad de Artes/Artes Visuales",
                IsDirectory = true
            });
            facultadArtes.Children.Add(new FileNode
            {
                Name = "Artes Musicales",
                Path = "/UABC/Ensenada/Facultad de Artes/Artes Musicales",
                IsDirectory = true
            });
            facultadArtes.Children.Add(new FileNode
            {
                Name = "Artes Teatrales",
                Path = "/UABC/Ensenada/Facultad de Artes/Artes Teatrales",
                IsDirectory = true
            });
            facultadArtes.Children.Add(new FileNode
            {
                Name = "Artes Literarias",
                Path = "/UABC/Ensenada/Facultad de Artes/Artes Literarias",
                IsDirectory = true
            });

            // Facultad de Ciencias
            var facultadCiencias = new FileNode
            {
                Name = "Facultad de Ciencias",
                Path = "/UABC/Ensenada/Facultad de Ciencias",
                IsDirectory = true
            };
            ensenada.Children.Add(facultadCiencias);

            facultadCiencias.Children.Add(new FileNode
            {
                Name = "Biologia",
                Path = "/UABC/Ensenada/Facultad de Ciencias/Biologia",
                IsDirectory = true
            });
            facultadCiencias.Children.Add(new FileNode
            {
                Name = "Matematicas",
                Path = "/UABC/Ensenada/Facultad de Ciencias/Matematicas",
                IsDirectory = true
            });
            facultadCiencias.Children.Add(new FileNode
            {
                Name = "Fisica",
                Path = "/UABC/Ensenada/Facultad de Ciencias/Fisica",
                IsDirectory = true
            });
            facultadCiencias.Children.Add(new FileNode
            {
                Name = "Quimica",
                Path = "/UABC/Ensenada/Facultad de Ciencias/Quimica",
                IsDirectory = true
            });

            // Facultad de Ciencias Marinas
            var facultadCienciasMarinas = new FileNode
            {
                Name = "Facultad de Ciencias Marinas",
                Path = "/UABC/Ensenada/Facultad de Ciencias Marinas",
                IsDirectory = true
            };
            ensenada.Children.Add(facultadCienciasMarinas);

            facultadCienciasMarinas.Children.Add(new FileNode
            {
                Name = "Biologia Marina",
                Path = "/UABC/Ensenada/Facultad de Ciencias Marinas/Biologia Marina",
                IsDirectory = true
            });
            facultadCienciasMarinas.Children.Add(new FileNode
            {
                Name = "Oceanografia",
                Path = "/UABC/Ensenada/Facultad de Ciencias Marinas/Oceanografia",
                IsDirectory = true
            });
            facultadCienciasMarinas.Children.Add(new FileNode
            {
                Name = "Ciencias del Mar",
                Path = "/UABC/Ensenada/Facultad de Ciencias Marinas/Ciencias del Mar",
                IsDirectory = true
            });
            facultadCienciasMarinas.Children.Add(new FileNode
            {
                Name = "Ingenieria en Ciencias del Mar",
                Path = "/UABC/Ensenada/Facultad de Ciencias Marinas/Ingenieria en Ciencias del Mar",
                IsDirectory = true
            });
            facultadCienciasMarinas.Children.Add(new FileNode
            {
                Name = "Ingenieria en Transporte Maritimo",
                Path = "/UABC/Ensenada/Facultad de Ciencias Marinas/Ingenieria en Transporte Maritimo",
                IsDirectory = true
            });

            // Facultad de Deportes
            var facultadDeportes = new FileNode
            {
                Name = "Facultad de Deportes",
                Path = "/UABC/Ensenada/Facultad de Deportes",
                IsDirectory = true
            };
            ensenada.Children.Add(facultadDeportes);

            facultadDeportes.Children.Add(new FileNode
            {
                Name = "Ciencias del Deporte",
                Path = "/UABC/Ensenada/Facultad de Deportes/Ciencias del Deporte",
                IsDirectory = true
            });
            facultadDeportes.Children.Add(new FileNode
            {
                Name = "Entrenamiento Deportivo",
                Path = "/UABC/Ensenada/Facultad de Deportes/Entrenamiento Deportivo",
                IsDirectory = true
            });
            facultadDeportes.Children.Add(new FileNode
            {
                Name = "Rehabilitacion y Terapia Fisica",
                Path = "/UABC/Ensenada/Facultad de Deportes/Rehabilitacion y Terapia Fisica",
                IsDirectory = true
            });
            facultadDeportes.Children.Add(new FileNode
            {
                Name = "Nutricion y Dietetica",
                Path = "/UABC/Ensenada/Facultad de Deportes/Nutricion y Dietetica",
                IsDirectory = true
            });
            facultadDeportes.Children.Add(new FileNode
            {
                Name = "Educacion Fisica",
                Path = "/UABC/Ensenada/Facultad de Deportes/Educacion Fisica",
                IsDirectory = true
            });

            // Facultad de Ingenieria
            var facultadIngenieria = new FileNode
            {
                Name = "Facultad de Ingenieria",
                Path = "/UABC/Ensenada/Facultad de Ingenieria",
                IsDirectory = true
            };
            ensenada.Children.Add(facultadIngenieria);

            facultadIngenieria.Children.Add(new FileNode
            {
                Name = "Tronco Comun de Ingenieria",
                Path = "/UABC/Ensenada/Facultad de Ingenieria/Tronco Comun de Ingenieria",
                IsDirectory = true
            });
            facultadIngenieria.Children.Add(new FileNode
            {
                Name = "Ingenieria Civil",
                Path = "/UABC/Ensenada/Facultad de Ingenieria/Ingenieria Civil",
                IsDirectory = true
            });
            facultadIngenieria.Children.Add(new FileNode
            {
                Name = "Ingenieria en Electronica",
                Path = "/UABC/Ensenada/Facultad de Ingenieria/Ingenieria en Electronica",
                IsDirectory = true
            });
            facultadIngenieria.Children.Add(new FileNode
            {
                Name = "Ingenieria en Computacion",
                Path = "/UABC/Ensenada/Facultad de Ingenieria/Ingenieria en Computacion",
                IsDirectory = true
            });
            facultadIngenieria.Children.Add(new FileNode
            {
                Name = "Ingenieria Industrial",
                Path = "/UABC/Ensenada/Facultad de Ingenieria/Ingenieria Industrial",
                IsDirectory = true
            });
            facultadIngenieria.Children.Add(new FileNode
            {
                Name = "Bioingenieria",
                Path = "/UABC/Ensenada/Facultad de Ingenieria/Bioingenieria",
                IsDirectory = true
            });

            return root;
        }
    }
}
