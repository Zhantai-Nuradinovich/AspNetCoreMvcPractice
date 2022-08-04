using AspNetCoreMvcPractice.Data.Models2;
using AspNetCoreMvcPractice.Services;
using AspNetCoreMvcPractice.ViewModels.Products;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Controllers
{
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

        public async Task<IActionResult> Index()
        {
            var products = await _categoryService.GetAllAsync();
            return View(products);
        }
    }
}
