using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.DTOs.Product;
using RolexWatches.Application.Interfaces.Services;

namespace RolexWatches.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET api/products?query=&brandId=&categoryId=&minPrice=&maxPrice=&inStockOnly=&sortBy=&pageNumber=&pageSize=
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] ProductFilterDto filter)
        {
            var result = await _productService.SearchAsync(filter);
            return Ok(result);
        }

        // GET api/products/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product is null ? NotFound() : Ok(product);
        }

        // POST api/products  (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            try
            {
                var created = await _productService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT api/products/5  (Admin only)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            try
            {
                var updated = await _productService.UpdateAsync(id, dto);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE api/products/5  (Admin only)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        // PATCH api/products/5/stock  (Admin only) — Module 3: Inventory
        [HttpPatch("{id:int}/stock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockDto dto)
        {
            var updated = await _productService.UpdateStockAsync(id, dto.StockQuantity);
            return updated is null ? NotFound() : Ok(updated);
        }

        // GET api/products/low-stock  (Admin only) — Module 3: Inventory
        [HttpGet("low-stock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLowStock([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _productService.GetLowStockAsync(pageNumber, pageSize);
            return Ok(result);
        }
    }
}
