using AspNetCoreMvcPractice.Business.Services;
using AspNetCoreMvcPractice.Data.Models;
using AspNetCoreMvcPractice.ViewModels.Products;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _service;
        private readonly IMapper _mapper;
        private readonly int _maxAmountOfProducts;

        public ProductController(IProductService service, 
                                  IMapper mapper, 
                                  IConfiguration configuration)
        {
            _service = service;
            _mapper = mapper;
            _maxAmountOfProducts = configuration.GetValue<int>("MaxAmountOfProducts");
        }

        /// <summary>
        /// Get all Products
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _service.GetMaxAmountAsync(_maxAmountOfProducts);
            return Ok(products);
        }

        /// <summary>
        /// Create a Product
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(_mapper.Map<Product>(product));
                return CreatedAtAction("Create", Task.CompletedTask);
            }

            return BadRequest();
        }

        /// <summary>
        /// Update a Product
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Edit(EditProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(_mapper.Map<Product>(product));
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete a specific Product
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id >= 1)
            {
                var product = await _service.GetByIdAsync(id);
                await _service.DeleteAsync(product);
                return Ok();
            }

            return BadRequest();
        }
    }
}
