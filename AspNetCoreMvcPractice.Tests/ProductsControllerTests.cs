using AspNetCoreMvcPractice.Business.Services;
using AspNetCoreMvcPractice.Controllers;
using AspNetCoreMvcPractice.Data.Models;
using AspNetCoreMvcPractice.Helpers;
using AspNetCoreMvcPractice.ViewModels.Products;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreMvcPractice.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;
        private readonly IMapper _mapper;

        public ProductsControllerTests()
        {
            _mockService = new Mock<IProductService>();

            var inMemorySettings = new Dictionary<string, string> { { "MaxAmountOfProducts", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            
            var myProfile = new MappingProfile();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(mapperConfig);

            _controller = new ProductsController(_mockService.Object, configuration, _mapper);
        }

        [Fact]
        public async Task Index_ReturnAViewResult_WithAllProductsAsync()
        {
            _mockService.Setup(serv => serv.GetMaxAmountAsync(5))
                        .ReturnsAsync(GetTestProducts());

            var result = await _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(
                viewResult.ViewData.Model);

            Assert.Equal(3, model.Count());
        }

        [Fact]
        public async Task Create_CreateNewProduct_ReturnsViewWithNewProductAsync()
        {
            var product = new Product() { ProductID = 1 };
            _mockService.Setup(serv => serv.CreateAsync(product));
            _mockService.Setup(serv => serv.GetMaxAmountAsync(5))
                .ReturnsAsync(new[] { product });

            var result = await _controller.Create(_mapper.Map<CreateProductViewModel>(product));

            result = await _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(
                viewResult.ViewData.Model);

            Assert.Single(model);
        }

        [Fact]
        public async Task Edit_EditProduct_ReturnsViewWithUpdatedProductAsync()
        {
            var product = new Product() { ProductID = 1 };
            _mockService.Setup(serv => serv.UpdateAsync(product));
            _mockService.Setup(serv => serv.GetMaxAmountAsync(5))
                .ReturnsAsync(new[] { product });

            product.ProductName = "Past tense";
            var result = await _controller.Edit(_mapper.Map<EditProductViewModel>(product));
            result = await _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(
                viewResult.ViewData.Model);
            Assert.Single(model);
            Assert.Equal("Past tense" , model.FirstOrDefault().ProductName);
        }

        private IEnumerable<Product> GetTestProducts()
        {
            return new[]
{
                new Product() { CategoryId = 1, ProductID = 1},
                new Product() { CategoryId = 2, ProductID = 2},
                new Product() { CategoryId = 3, ProductID = 3}
            };
        }
    }
}
