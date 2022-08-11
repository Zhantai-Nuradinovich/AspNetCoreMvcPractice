using AspNetCoreMvcPractice.Business.Services;
using AspNetCoreMvcPractice.ViewModels.Categories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCoreMvcPractice.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all Categories
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Get specific category image
        /// </summary>
        /// <returns>An array of bytes</returns>
        [HttpGet("image/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var picture = await _service.GetPictureByIdAsync(id);
            return File(picture, "image/bmp");
        }

        /// <summary>
        /// Update specific category image
        /// </summary>
        /// <returns>A CategoryImageModel</returns>
        [HttpPut("image")]
        public async Task<IActionResult> EditImage(EditImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.EditImageById(model.CategoryID, model.Picture);
                return Ok();
            }

            return BadRequest();
        }
    }
}
