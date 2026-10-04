using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;

namespace RolexWatches.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		// GET api/categories
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				return Ok(await _categoryService.GetAllAsync());
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}
		}

		// GET api/categories/5
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var category = await _categoryService.GetByIdAsync(id);
				return category is null ? NotFound() : Ok(category);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}
		}

		// POST api/categories  (Admin only)
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
		{
			try
			{
				var created = await _categoryService.CreateAsync(dto);
				return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
			}
			catch (InvalidOperationException ex)
            {

				return BadRequest(ex.Message);
			}
		}

		// PUT api/categories/5  (Admin only)
		[HttpPut("{id:int}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(int id, [FromBody] CreateCategoryDto dto)
		{
			try
			{
				var updated = await _categoryService.UpdateAsync(id, dto);
				return updated ? NoContent() : NotFound();
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}
		}

		// DELETE api/categories/5  (Admin only)
		[HttpDelete("{id:int}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var deleted = await _categoryService.DeleteAsync(id);
				return deleted ? NoContent() : NotFound();
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}
		}
	}
}