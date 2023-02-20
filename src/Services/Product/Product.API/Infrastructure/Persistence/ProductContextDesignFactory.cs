using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Product.API.Infrastructure.Persistence
{
    public class ProductContextDesignFactory : IDesignTimeDbContextFactory<ProductContext>
    {
        public ProductContext CreateDbContext(string[] args)
        {
            string connectionString = GetConnectionString(args);

            Console.WriteLine(connectionString);

            DbContextOptionsBuilder<ProductContext> optionsBuilder = new DbContextOptionsBuilder<ProductContext>()
                .UseSqlServer(connectionString);

            return new ProductContext(optionsBuilder.Options);
        }

        private static string GetConnectionString(string[] args)
        {
            if (args.Length < 4)
            {
                return "Server=.;Initial Catalog=product-db;Integrated Security=true";
            }

            string server = args[0];
            string databaseName = args[1];
            string userId = args[2];
            string password = args[3];

            return $"Server={server};Initial Catalog={databaseName};User Id={userId};Password={password}";
        }
    }
}
