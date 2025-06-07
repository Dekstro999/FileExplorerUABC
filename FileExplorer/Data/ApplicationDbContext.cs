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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar la relación jerárquica (self-referencing)
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

                // Índice para mejorar el rendimiento
                entity.HasIndex(e => e.ParentId);
                entity.HasIndex(e => new { e.ParentId, e.Name }).IsUnique();
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
