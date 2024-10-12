using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        public CategoryController(IServiceContainer service) : base(service)
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO dTO)
        {
            var result = await serviceContainer.Category.AddCategory(dTO);

            if (result)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        //PostManURL = https://localhost:7088/api/Category/id
        [HttpGet("{id}")]
        public async Task<IActionResult> FindCategory([FromRoute]int id)
        {
            var result = await serviceContainer.Category.FindCategory(id);
            if (result !=null)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpGet()]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            var result = await serviceContainer.Category.DeleteCategory(id);
            if (result)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var result = await serviceContainer.Category.GetAllCategories();
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateCategory(long id, [FromBody] CategoryDTO categoryDTO)
        {
            var result = await serviceContainer.Category.UpdateCategory(id, categoryDTO);
            if (result)
            {
                return Ok("Category updated successfully");
            }
            return BadRequest("Error updating category");
        }


    }
}
