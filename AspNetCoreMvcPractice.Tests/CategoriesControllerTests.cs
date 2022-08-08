using AspNetCoreMvcPractice.Business.Services;
using AspNetCoreMvcPractice.Controllers;
using AspNetCoreMvcPractice.Data.Models;
using AspNetCoreMvcPractice.Helpers;
using AspNetCoreMvcPractice.ViewModels.Categories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreMvcPractice.Tests
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _mockService;
        private readonly Mapper _mapper;
        private readonly CategoriesController _controller;
        public CategoriesControllerTests()
        {
            _mockService = new Mock<ICategoryService>();

            var myProfile = new MappingProfile();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(mapperConfig);

            _controller = new CategoriesController(_mockService.Object, _mapper);
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAllCategoriesAsync()
        {
            _mockService.Setup(serv => serv.GetAllAsync())
                .ReturnsAsync(GetTestCategories());

            var result = await _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Category>>(
                viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public async Task Edit_EditCategoryImage_ReturnsViewWithUpdatedCategoryImageAsync()
        {
            var picture = new byte[] { 1, 54, 57, 77, 44, 1 };
            var category = new Category() { CategoryID= 1 , Picture = picture};
            _mockService.Setup(serv => serv.EditImageById(category.CategoryID, category.Picture));
            _mockService.Setup(serv => serv.GetByIdAsync(1))
                .ReturnsAsync(category);
            using var stream = new MemoryStream();
            var formFile = new FormFile(stream, 0, stream.Length, "streamFile", "temp");

            var result = await _controller.EditImage(_mapper.Map<EditImageViewModel>(category), formFile);
            result = await _controller.EditImage(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EditImageViewModel>(
                viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.NotEmpty(model.Picture);
        }

        private IEnumerable<Category> GetTestCategories()
        {
            return new [] 
            { 
                new Category() { CategoryID = 1, CategoryName = "First", Description = "AA"},
                new Category() { CategoryID = 2, CategoryName = "Second", Description = "BB"},
                new Category() { CategoryID = 3, CategoryName = "Third", Description = "CC"}
            };
        }
    }
}
