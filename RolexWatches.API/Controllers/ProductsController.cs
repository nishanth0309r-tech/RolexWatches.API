using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;

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

		// GET api/products
		[HttpGet]
		public async Task<IActionResult> GetAll() => Ok(await _productService.GetAllAsync());

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
			var created = await _productService.CreateAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		// PUT api/products/5  (Admin only)
		[HttpPut("{id:int}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
		{
			var updated = await _productService.UpdateAsync(id, dto);
			return updated ? NoContent() : NotFound();
		}

		// DELETE api/products/5  (Admin only)
		[HttpDelete("{id:int}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var deleted = await _productService.DeleteAsync(id);
			return deleted ? NoContent() : NotFound();
		}
	}
}