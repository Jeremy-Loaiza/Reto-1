using Microsoft.EntityFrameworkCore;
using RetoBackend.Models;

namespace RetoBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // 📦 DbSet que representa la tabla Jeremy_Reto.Recaudos
        public DbSet<RecaudoEntity> Recaudos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 Configuración explícita del esquema y nombre de tabla
            modelBuilder.Entity<RecaudoEntity>(entity =>
            {
                entity.ToTable("Recaudos", schema: "Jeremy_Reto");

                // 🔹 Propiedades mapeadas correctamente con validaciones del modelo
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Fecha).IsRequired();
                entity.Property(e => e.EstacionNombre)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Sentido)
                      .HasMaxLength(50);

                entity.Property(e => e.Categoria)
                      .HasMaxLength(50);

                entity.Property(e => e.Hora)
                      .HasColumnType("int");

                entity.Property(e => e.Cantidad)
                      .IsRequired();

                entity.Property(e => e.Valor)
                      .HasColumnType("decimal(18,2)");
            });
        }
    }
}
