using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarifasElectricas.Domain.Entities;

namespace TarifasElectricas.Persistence.Configurations;

public class EtlLogConfiguration : IEntityTypeConfiguration<EtlLog>
{
    public void Configure(EntityTypeBuilder<EtlLog> builder)
    {
        builder.ToTable("etl_logs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ExecutionDate).IsRequired();
        builder.Property(x => x.State)
            .HasConversion<int>()
            .IsRequired();
        builder.Property(x => x.Message).HasMaxLength(1000);
    }
}
