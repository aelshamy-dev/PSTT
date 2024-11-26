

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Configuration;

public class CompanyConfiguration: IEntityTypeConfiguration<Company.Domain.Models.Company>
{
    public void Configure(EntityTypeBuilder<Company.Domain.Models.Company> builder)
    {
        builder.HasKey(c => c.CompanyId);

        builder.Property(c => c.CompanyName).HasMaxLength(200).IsRequired();
    }
}
