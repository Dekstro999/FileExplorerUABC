using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FileExplorer.Migrations.ContextoBDMigrations
{
    /// <inheritdoc />
    public partial class InitialCreateContextoBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileNodes",
                columns: table => new
                {
                    Path = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDirectory = table.Column<bool>(type: "bit", nullable: false),
                    IsExpanded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileNodes", x => x.Path);
                });

            migrationBuilder.InsertData(
                table: "FileNodes",
                columns: new[] { "Path", "IsDirectory", "IsExpanded", "Name" },
                values: new object[,]
                {
                    { "/UABC", true, true, "UABC" },
                    { "/UABC/Ensenada", true, true, "Ensenada" },
                    { "/UABC/Ensenada/Facultad de Artes", true, false, "Facultad de Artes" },
                    { "/UABC/Ensenada/Facultad de Artes/Artes Literarias", true, false, "Artes Literarias" },
                    { "/UABC/Ensenada/Facultad de Artes/Artes Musicales", true, false, "Artes Musicales" },
                    { "/UABC/Ensenada/Facultad de Artes/Artes Teatrales", true, false, "Artes Teatrales" },
                    { "/UABC/Ensenada/Facultad de Artes/Artes Visuales", true, false, "Artes Visuales" },
                    { "/UABC/Ensenada/Facultad de Ciencias", true, false, "Facultad de Ciencias" },
                    { "/UABC/Ensenada/Facultad de Ciencias Marinas", true, false, "Facultad de Ciencias Marinas" },
                    { "/UABC/Ensenada/Facultad de Ciencias Marinas/Biologia Marina", true, false, "Biologia Marina" },
                    { "/UABC/Ensenada/Facultad de Ciencias Marinas/Ciencias del Mar", true, false, "Ciencias del Mar" },
                    { "/UABC/Ensenada/Facultad de Ciencias Marinas/Ingenieria en Ciencias del Mar", true, false, "Ingenieria en Ciencias del Mar" },
                    { "/UABC/Ensenada/Facultad de Ciencias Marinas/Ingenieria en Transporte Maritimo", true, false, "Ingenieria en Transporte Maritimo" },
                    { "/UABC/Ensenada/Facultad de Ciencias Marinas/Oceanografia", true, false, "Oceanografia" },
                    { "/UABC/Ensenada/Facultad de Ciencias/Biologia", true, false, "Biologia" },
                    { "/UABC/Ensenada/Facultad de Ciencias/Fisica", true, false, "Fisica" },
                    { "/UABC/Ensenada/Facultad de Ciencias/Matematicas", true, false, "Matematicas" },
                    { "/UABC/Ensenada/Facultad de Ciencias/Quimica", true, false, "Quimica" },
                    { "/UABC/Ensenada/Facultad de Deportes", true, false, "Facultad de Deportes" },
                    { "/UABC/Ensenada/Facultad de Deportes/Ciencias del Deporte", true, false, "Ciencias del Deporte" },
                    { "/UABC/Ensenada/Facultad de Deportes/Educacion Fisica", true, false, "Educacion Fisica" },
                    { "/UABC/Ensenada/Facultad de Deportes/Entrenamiento Deportivo", true, false, "Entrenamiento Deportivo" },
                    { "/UABC/Ensenada/Facultad de Deportes/Nutricion y Dietetica", true, false, "Nutricion y Dietetica" },
                    { "/UABC/Ensenada/Facultad de Deportes/Rehabilitacion y Terapia Fisica", true, false, "Rehabilitacion y Terapia Fisica" },
                    { "/UABC/Ensenada/Facultad de Ingenieria", true, false, "Facultad de Ingenieria" },
                    { "/UABC/Ensenada/Facultad de Ingenieria/Bioingenieria", true, false, "Bioingenieria" },
                    { "/UABC/Ensenada/Facultad de Ingenieria/Ingenieria Civil", true, false, "Ingenieria Civil" },
                    { "/UABC/Ensenada/Facultad de Ingenieria/Ingenieria en Computacion", true, false, "Ingenieria en Computacion" },
                    { "/UABC/Ensenada/Facultad de Ingenieria/Ingenieria en Electronica", true, false, "Ingenieria en Electronica" },
                    { "/UABC/Ensenada/Facultad de Ingenieria/Ingenieria Industrial", true, false, "Ingenieria Industrial" },
                    { "/UABC/Ensenada/Facultad de Ingenieria/Tronco Comun de Ingenieria", true, false, "Tronco Comun de Ingenieria" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileNodes");
        }
    }
}
