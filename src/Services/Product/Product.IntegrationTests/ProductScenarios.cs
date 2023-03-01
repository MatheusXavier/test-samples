using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Product.API.ViewModels;
using Product.IntegrationTests.Base;

using System.Net;

namespace Product.IntegrationTests;

public class ProductScenarios : ProductScenariosBase
{
    [Fact]
    public async Task Get_all_product_items_and_response_status_code_ok()
    {
        // Arrange
        using ProductTestServer server = CreateServer();
        using HttpClient httpClient = server.CreateClient();

        // Act
        HttpResponseMessage response = await httpClient.GetAsync("api/v1/products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_product_by_id_and_response_status_code_notfound()
    {
        // Arrange
        using ProductTestServer server = CreateServer();
        using HttpClient httpClient = server.CreateClient();

        // Act
        HttpResponseMessage response = await httpClient
            .GetAsync($"api/v1/products/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_product_by_id_and_response_status_code_ok()
    {
        // Arrange
        using ProductTestServer server = CreateServer();
        using HttpClient httpClient = server.CreateClient();

        API.Domain.Entities.Product product = new(
            Guid.NewGuid(),
            name: "Product name",
            value: 1.2,
            active: true);

        await server.ProductContext.Products.AddAsync(product);
        await server.ProductContext.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await httpClient
            .GetAsync($"api/v1/products/{product.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        API.Domain.Entities.Product? content =
            await GetRequestContent<API.Domain.Entities.Product>(response);

        content.Should().NotBeNull();
        content.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task Post_create_a_new_product_and_response_status_code_ok()
    {
        // Arrange
        using ProductTestServer server = CreateServer();
        using HttpClient httpClient = server.CreateClient();

        CreateProductViewModel productToCreate = new("New Product Name", 1.2, true);
        StringContent requestContent = BuildRequestContent(productToCreate);

        // Act
        HttpResponseMessage response = await httpClient
            .PostAsync("api/v1/products", requestContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        API.Domain.Entities.Product? content =
            await GetRequestContent<API.Domain.Entities.Product>(response);

        content.Should().NotBeNull();

        API.Domain.Entities.Product? dbProduct =
            await server.ProductContext.Products.FindAsync(content?.Id);

        dbProduct.Should().NotBeNull();
        dbProduct.Should().BeEquivalentTo(content);
    }

    [Fact]
    public async Task Put_update_product_and_response_status_code_notfound()
    {
        // Arrange
        using ProductTestServer server = CreateServer();
        using HttpClient httpClient = server.CreateClient();

        UpdateProductViewModel productToUpdated = new(
            Guid.NewGuid(),
            "Updated name",
            1.0);
        StringContent requestContent = BuildRequestContent(productToUpdated);

        // Act
        HttpResponseMessage response = await httpClient
            .PutAsync("api/v1/products", requestContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Put_update_product_and_response_status_code_ok()
    {
        // Arrange
        using ProductTestServer server = CreateServer();
        using HttpClient httpClient = server.CreateClient();

        API.Domain.Entities.Product product = new(
            Guid.NewGuid(),
            name: "Product name",
            value: 1.2,
            active: true);

        await server.ProductContext.Products.AddAsync(product);
        await server.ProductContext.SaveChangesAsync();

        UpdateProductViewModel productToUpdated = new(
            product.Id, "Updated name (1)", 100.34);
        StringContent requestContent = BuildRequestContent(productToUpdated);

        // Act
        HttpResponseMessage response = await httpClient
            .PutAsync("api/v1/products", requestContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        API.Domain.Entities.Product? dbProduct = await server.ProductContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        dbProduct.Should().NotBeNull();
        dbProduct.Should().BeEquivalentTo(new API.Domain.Entities.Product(
            product.Id,
            "Updated name (1)",
            100.34,
            active: true));
    }
}