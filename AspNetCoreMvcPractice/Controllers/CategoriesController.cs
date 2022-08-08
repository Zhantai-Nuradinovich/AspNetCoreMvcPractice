using AspNetCoreMvcPractice.Business.Services;
using AspNetCoreMvcPractice.Helpers;
using AspNetCoreMvcPractice.ViewModels.Categories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Controllers
{
    [LogAction]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(
            ICategoryService categoryService,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _categoryService.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        [Route("images/{id}")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            return File(await _categoryService.GetPictureByIdAsync(id), "image/bmp");
        }

        [HttpGet]
        public async Task<IActionResult> EditImage(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return View(_mapper.Map<EditImageViewModel>(category));
        }

        [HttpPost] //с PUT не работает
        public async Task<IActionResult> EditImage(EditImageViewModel model, IFormFile uploadedFile)
        {
            if (ModelState.IsValid && uploadedFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await uploadedFile.CopyToAsync(memoryStream);
                    model.Picture = memoryStream.ToArray();
                    await _categoryService.EditImageById(model.CategoryID, model.Picture);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
