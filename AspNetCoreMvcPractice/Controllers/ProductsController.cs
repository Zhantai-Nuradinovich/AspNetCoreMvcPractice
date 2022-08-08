using AspNetCoreMvcPractice.Business.Services;
using AspNetCoreMvcPractice.Data.Models;
using AspNetCoreMvcPractice.Helpers;
using AspNetCoreMvcPractice.ViewModels.Products;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Controllers
{
    [LogAction]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly int _maxAmountOfProducts;

        public ProductsController(
            IProductService productService,
            IConfiguration configuration,
            IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
            _maxAmountOfProducts = configuration.GetValue<int>("MaxAmountOfProducts");
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetMaxAmountAsync(_maxAmountOfProducts);
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await FillProductsDropDownLists();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateAsync(_mapper.Map<Product>(product));
                return RedirectToAction("Index");
            }
            else
            {
                await FillProductsDropDownLists();
                return View(product);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            await FillProductsDropDownLists();
            var product = await _productService.GetByIdAsync(id);

            return View(_mapper.Map<EditProductViewModel>(product));
        }

        [HttpPut]
        public async Task<IActionResult> Edit(EditProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateAsync(_mapper.Map<Product>(product));
                return RedirectToAction("Index");
            }
            else
            {
                await FillProductsDropDownLists();
                return View(product);
            }
        }

        private async Task FillProductsDropDownLists()
        {
            ViewBag.Suppliers = new SelectList(await _productService.GetSuppliers(), "SupplierId", "CompanyName");
            ViewBag.Categories = new SelectList(await _productService.GetCategories(), "CategoryId", "CategoryName");
        }
    }
}
