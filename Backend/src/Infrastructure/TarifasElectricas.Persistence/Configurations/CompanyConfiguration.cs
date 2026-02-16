using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarifasElectricas.Domain.Entities;

namespace TarifasElectricas.Persistence.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("companies");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Code).IsRequired().HasMaxLength(300);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.DateUpdated).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
