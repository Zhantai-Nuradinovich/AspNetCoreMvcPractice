using AspNetCoreMvcPractice.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _categoryService.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            return File(await _categoryService.GetPictureByIdAsync(id), "image/bmp");
        }
    }
}
