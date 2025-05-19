using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MerchStore.WebUI.Models.Api.Basic;
using MerchStore.Application.Services.Interfaces;

namespace MerchStore.WebUI.Controllers.Api.Products;

/// <summary>
/// Basic API controller for read-only product operations.
/// Requires API Key authentication.
/// </summary>
[Route("api/basic/products")]
[ApiController]
[Authorize(Policy = "ApiKeyPolicy")]
public class BasicProductsApiController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public BasicProductsApiController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    /// <summary>
    /// Gets all products
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BasicProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _catalogService.GetAllProductsAsync();

            var productDtos = products.Select(p => new BasicProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,                  // ✅ now a decimal
                Currency = "SEK",                 // ✅ hardcoded string
                ImageUrl = p.ImageUrl?.ToString(),
                StockQuantity = p.StockQuantity
            });

            return Ok(productDtos);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while retrieving products" });
        }
    }

    /// <summary>
    /// Gets a specific product by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BasicProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var product = await _catalogService.GetProductByIdAsync(id);

            if (product is null)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }

            var productDto = new BasicProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,            // ✅ now a decimal
                Currency = "SEK",                 // ✅ hardcoded string
                ImageUrl = product.ImageUrl?.ToString(),
                StockQuantity = product.StockQuantity
            };

            return Ok(productDto);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while retrieving the product" });
        }
    }
}
