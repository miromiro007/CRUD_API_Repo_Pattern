 using CRUD_API.Models;
using CRUD_API.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        IUnitWork _unitWork;

        public CategoriesController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        [HttpGet]
        public Task<IActionResult> GetAll()
        {
            var categories = _unitWork.Categories.GetAllAsync();
            return Task.FromResult<IActionResult>(Ok(categories));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Les données sont requises");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _unitWork.Categories.AddAsync(category);
            await _unitWork.CommitChangeAsync();
            return CreatedAtAction(nameof(GetAll), new { id = category.Id }, category);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _unitWork.Categories.TfindByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unitWork.Categories.TfindByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            await _unitWork.Categories.DeleteAsync(id);
            await _unitWork.CommitChangeAsync();
            return NoContent();
        }
    }

}
