using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Product.API.Infrastructure.Persistence;
using Product.API.ViewModels;

namespace Product.API.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _productContext;

        public ProductsController(ProductContext productContext)
        {
            _productContext = productContext;
        }

        [HttpGet("api/v1/products/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Domain.Entities.Product), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            Domain.Entities.Product? product = await _productContext.Products
                .SingleOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("api/v1/products")]
        [ProducesResponseType(typeof(List<Domain.Entities.Product>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllProductsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            List<Domain.Entities.Product> productsOnPage = await _productContext.Products
                .Skip(pageIndex * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return Ok(productsOnPage);
        }

        [HttpPost("api/v1/products")]
        [ProducesResponseType(typeof(Domain.Entities.Product), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateProductAsync(CreateProductViewModel createProductViewModel)
        {
            Domain.Entities.Product product = new(
                id: Guid.NewGuid(),
                createProductViewModel.Name,
                createProductViewModel.Value,
                createProductViewModel.Active);

            await _productContext.Products.AddAsync(product);
            await _productContext.SaveChangesAsync();

            return Created("api/v1/products", product);
        }

        [HttpPut("api/v1/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProductAsync(UpdateProductViewModel updateProductViewModel)
        {
            Domain.Entities.Product? product = await _productContext.Products
                .SingleOrDefaultAsync(p => p.Id == updateProductViewModel.Id);

            if (product is null)
            {
                return NotFound();
            }

            try
            {
                product.Update(updateProductViewModel.Name, updateProductViewModel.Value);

                _productContext.Products.Update(product);
                await _productContext.SaveChangesAsync();

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
