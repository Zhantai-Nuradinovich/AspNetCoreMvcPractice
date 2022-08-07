using AspNetCoreMvcPractice.Business.Services;
using AspNetCoreMvcPractice.Controllers;
using AspNetCoreMvcPractice.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreMvcPractice.Tests
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _mockService;
        private readonly CategoriesController _controller;
        public CategoriesControllerTests()
        {
            _mockService = new Mock<ICategoryService>();
            _controller = new CategoriesController(_mockService.Object);
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
