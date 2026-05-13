using Domain.Payrolls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

internal sealed class PayrollConfiguration : IEntityTypeConfiguration<Payroll>
{
    public void Configure(EntityTypeBuilder<Payroll> builder)
    {
        builder.HasKey(p => p.Id);

        builder.ComplexProperty(
            p => p.Email,
            b => b.Property(e => e.Value).HasColumnName("email"));

        builder.ComplexProperty(
            p => p.Name,
            b => b.Property(n => n.Value).HasColumnName("name"));

        builder.ComplexProperty(
            p => p.CompanyName,
            b => b.Property(n => n.Value).HasColumnName("company_name"));

        builder.Property(p => p.StartDate).HasColumnName("start_date");

        builder.Property(p => p.EndDate).HasColumnName("end_date");
    }
}
