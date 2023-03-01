using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using Product.API.Infrastructure.Persistence;

namespace Product.IntegrationTests.Base;

public class ProductTestServer : TestServer
{
    public ProductTestServer(IWebHostBuilder builder) : base(builder)
    {
        ProductContext = Host.Services.GetRequiredService<ProductContext>();
    }

    public ProductContext ProductContext { get; set; }
}
