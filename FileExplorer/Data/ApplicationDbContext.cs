using FileExplorer.Models;
using Microsoft.EntityFrameworkCore;

namespace FileExplorer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Folder> Folders { get; set; }
        public DbSet<Universidad> Universidades { get; set; }
        public DbSet<Campus> Campus { get; set; }
        public DbSet<Facultad> Facultades { get; set; }
        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Semestre> Semestres { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Contenido> Contenidos { get; set; }
        public DbSet<TipoArchivo> TiposArchivo { get; set; }
        public DbSet<RecursoContenido> RecursosContenido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración Folder (ya existente)
            modelBuilder.Entity<Folder>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasMany(f => f.Children)
                    .WithOne(f => f.Parent)
                    .HasForeignKey(f => f.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.ParentId);
                entity.HasIndex(e => new { e.ParentId, e.Name }).IsUnique();
            });

            // Configuración Contenido
            modelBuilder.Entity<Contenido>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(e => e.Materia)
                    .WithMany(m => m.Contenidos)
                    .HasForeignKey(e => e.MateriaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración TipoArchivo
            modelBuilder.Entity<TipoArchivo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Extension).HasMaxLength(10);
                entity.Property(e => e.MimeType).HasMaxLength(100);
            });

            // Configuración RecursoContenido
            modelBuilder.Entity<RecursoContenido>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Url).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Contenido)
                    .WithMany(c => c.RecursosContenido)
                    .HasForeignKey(e => e.ContenidoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.TipoArchivo)
                    .WithMany(t => t.RecursosContenido)
                    .HasForeignKey(e => e.TipoArchivoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Cadena de conexión de respaldo si no se configura en Program.cs
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FileExplorerDb;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}
