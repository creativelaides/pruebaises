using Microsoft.EntityFrameworkCore;
using TarifasElectricas.Domain.Entities;

namespace TarifasElectricas.Infrastructure.Data
{
    public class TarifasDbContext : DbContext
    {
        public TarifasDbContext(DbContextOptions<TarifasDbContext> options) : base(options)
        {
        }

        public DbSet<TarifaElectrica> TarifasElectricas { get; set; }
        public DbSet<EducationComponent> ComponentesEducacion { get; set; }
        public DbSet<EtlLog> EtlLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TarifaElectrica entity
            modelBuilder.Entity<TarifaElectrica>(entity =>
            {
                entity.HasIndex(e => new { e.Periodo, e.Nivel }).IsUnique();
                entity.Property(e => e.CuTotal).HasColumnType("numeric(10, 4)");
                entity.Property(e => e.CostoCompraG).HasColumnType("numeric(10, 4)");
                entity.Property(e => e.CargoTransporteStnTm).HasColumnType("numeric(10, 4)");
                entity.Property(e => e.CargoTransporteSdlDm).HasColumnType("numeric(10, 4)");
                entity.Property(e => e.MargenComercializacion).HasColumnType("numeric(10, 4)");
                entity.Property(e => e.CostoPerdidasPr).HasColumnType("numeric(10, 4)");
                entity.Property(e => e.RestriccionesRm).HasColumnType("numeric(10, 4)");
                entity.Property(e => e.Cot).HasColumnType("numeric(10, 4)");
                entity.Property(e => e.CfmjGfact).HasColumnType("numeric(10, 4)");
            });

            // Configure EtlLog entity
            modelBuilder.Entity<EtlLog>(entity =>
            {
                entity.Property(e => e.DurationSeconds).HasColumnType("numeric(10, 2)");
            });

        }
    }
}
