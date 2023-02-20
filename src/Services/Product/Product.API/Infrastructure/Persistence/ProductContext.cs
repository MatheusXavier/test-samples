using Microsoft.EntityFrameworkCore;

using Product.API.Infrastructure.Persistence.EntityConfigurations;

namespace Product.API.Infrastructure.Persistence
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new ProductEntityTypeConfiguration());
        }
    }
}
