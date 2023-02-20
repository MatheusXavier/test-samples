using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.API.Infrastructure.Persistence.EntityConfigurations
{
    public class ProductEntityTypeConfiguration
        : IEntityTypeConfiguration<Domain.Entities.Product>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Product> builder)
        {
            builder.ToTable("Products");

            builder
                .HasKey(x => x.Id)
                .IsClustered(false)
                .HasName("PK_Product");

            builder
                .Property(c => c.Id)
                .ValueGeneratedNever()
                .HasDefaultValueSql("newsequentialid()")
                .IsRequired();

            builder
                .Property(c => c.Name)
                .HasColumnType("nvarchar(250)")
                .IsRequired();
        }
    }
}
