using CRUD_API.Models;
using CRUD_API.Repository.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {

        private readonly IUnitWork _unitWork;

        public ItemsController(IUnitWork unitWork)
        {

            _unitWork = unitWork;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var items = await _unitWork.Items.GetAllAsync();
            return Ok(items);
        }

        // Implement other CRUD operations (Get by ID, Create, Update, Delete) similarly
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _unitWork.Items.TfindByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateItem([FromBody] Item item)
        {
            if (item == null)
            {
                return BadRequest("Les données sont requises");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _unitWork.Items.AddAsync(item);
            await _unitWork.CommitChangeAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }
        [HttpPut]
        [Route("edit/{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] Item item)
        {
            if (item == null)
            {
                return BadRequest("Les données sont requises");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingItem = await _unitWork.Items.TfindByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            // Update the item properties
            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.CategoryId = item.CategoryId;

            await _unitWork.Items.UpdateAsync(existingItem);
            await _unitWork.CommitChangeAsync();

            return Ok(new { message = "Item mise a jour avec succes", existingItem });
        }

        [HttpPatch]
        [Route("edit/{id}")]
        public async Task<IActionResult> PartialEdit(int id, [FromBody] Item item)
        {
            if (item == null)
            {
                return BadRequest(new { message = "Les données sont requises" });
            }

            /*if (!ModelState.IsValid)
            {
            }    return BadRequest(ModelState);*/  // u must use it if you want to validate the model, but in this case we will not validate it because we want to update only some properties


            var existingItem = await _unitWork.Items.TfindByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound(new { message = "L'élément n'existe pas" });
            }

            // Update only the properties that are provided (non-null/non-empty)
            if (!string.IsNullOrEmpty(item.Name))
            {
                existingItem.Name = item.Name;
            }

            if (!string.IsNullOrEmpty(item.Description))
            {
                existingItem.Description = item.Description;
            }



            // Update CategoryId if provided and verify the category exists
            if (item.CategoryId.HasValue)
            {
                var category = await _unitWork.Categories.TfindByIdAsync(item.CategoryId.Value);
                if (category == null)
                {
                    return BadRequest(new { message = "La catégorie spécifiée n'existe pas" });
                }
                existingItem.CategoryId = item.CategoryId;
            }

            await _unitWork.Items.UpdateAsync(existingItem);
            await _unitWork.CommitChangeAsync();

            return Ok(new { message = "Item partiellement mis à jour avec succès", item = existingItem });
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var existingItem = _unitWork.Items.TfindById(id);
            /*var existingItem = await _unitWork.Items.TfindByIdAsync(id);*/
            if (existingItem == null)
            {
                return NotFound();
            }

            await _unitWork.Items.DeleteAsync(id);
            await _unitWork.CommitChangeAsync();

            return Ok(new { message = "L'élément a été supprimé avec succès", item = existingItem });
        }

        [HttpGet]
        [Route("search")]
        public IActionResult SearchByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Le nom est requis");
            }

            var items = _unitWork.Items.SelectMany(i => i.Name.ToLower().Contains(name.ToLower()));
            if (!items.Any())
            {
                return NotFound(new { message = "Aucun élément trouvé avec ce nom" });
            }

            return Ok(items);
        }

        [HttpGet]
        [Route("searchByCategory/{id}")]
        public async Task<IActionResult> SearchByCategory(int id)
        {
            var category = await _unitWork.Categories.TfindByIdAsync(id);
            if (category == null)
            {
                return BadRequest("La catégorie spécifiée n'existe pas");
            }

            var items = _unitWork.Items.SelectMany(i => i.CategoryId == id);
            if (!items.Any())
            {
                return NotFound(new { message = "Aucun élément trouvé pour cette catégorie" });
            }
            return Ok(items);
        }
    }
        

    
}
