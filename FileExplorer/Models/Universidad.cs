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

        public Semestre Semestres{ get; set; } 
    }
}
