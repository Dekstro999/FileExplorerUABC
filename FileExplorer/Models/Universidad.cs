using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileExplorer.Models
{
    public class Universidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public ICollection<Campus> Campus { get; set; }
    }

    public class Campus
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int UniversidadId { get; set; }
        public Universidad Universidad { get; set; }
        public ICollection<Facultad> Facultades { get; set; }
    }

    public class Facultad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CampusId { get; set; }
        public Campus Campus { get; set; }
        public ICollection<Carrera> Carreras { get; set; }
    }

    public class Carrera
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int FacultadId { get; set; }
        public Facultad Facultad { get; set; }
        public ICollection<Semestre> Semestres { get; set; }
    }

    public class Semestre
    {
        //Id INT PRIMARY KEY IDENTITY(1,1),
        //Numero INT NOT NULL,
        //CarreraId INT NOT NULL,
        //FOREIGN KEY(CarreraId) REFERENCES Carreras(Id)
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int CarreraId { get; set; }

        public Carrera Carrera { get; set; }


        public ICollection<Materia> Materias { get; set; }

    }

    public class Materia
    {
//    CREATE TABLE Materias(
//      Id INT PRIMARY KEY IDENTITY(1,1),
//      Nombre NVARCHAR(100) NOT NULL,
//      CarreraId INT NOT NULL,
//      FOREIGN KEY(CarreraId) REFERENCES Carreras(Id)
//);
    
        public int Id { get; set; }
        public string Nombre { get; set;} = string.Empty;

        public int SemestreId { get; set; }

        public Semestre Semestre{ get; set; }

        public ICollection<Contenido> Contenidos { get; set; }
    }

    public class Contenido
    {
        public int Id { get; set; }
        public int MateriaId { get; set; }
        public string Numero { get; set; } = string.Empty; // Ejemplo: '1.1', '2.4'
        public string Titulo { get; set; } = string.Empty; // Ejemplo: 'Historia de los patrones de software'

        public Materia Materia { get; set; }

        public ICollection<RecursoContenido> RecursosContenido { get; set; } 
    }

    public class TipoArchivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Extension { get; set; }
        public string MimeType { get; set; }

        public ICollection<RecursoContenido> RecursosContenido { get; set; }
    }

    public class RecursoContenido
    {
        public int Id { get; set; }
        public int ContenidoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int TipoArchivoId { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Contenido Contenido { get; set; }
        public TipoArchivo TipoArchivo { get; set; }
    }
}
