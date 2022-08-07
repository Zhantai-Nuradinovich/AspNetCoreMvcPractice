using AspNetCoreMvcPractice.Data.Interfaces;
using AspNetCoreMvcPractice.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IRepository<Category> _categoryRepository;

        public ProductService(
            IProductRepository productRepository,
            IRepository<Supplier> supplierRepository,
            IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Product>> GetMaxAmountAsync(int maxAmountOfProducts)
        {
            if (maxAmountOfProducts == 0)
                return await _productRepository.FindAllAsync();

            return await _productRepository.TakeAsync(maxAmountOfProducts);
        }

        public async Task CreateAsync(Product product)
        {
            await _productRepository.AddAsync(product);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var model = await _productRepository.FindAsync(p => p.ProductID == id);
            if(model == null)
                throw new InvalidOperationException($"Product with Id: {id} not found");

            return model;
        }

        public async Task UpdateAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(Product product)
        {
            await _productRepository.DeleteAsync(product);
        }

        public async Task<IEnumerable<Supplier>> GetSuppliers()
        {
            return await _supplierRepository.FindAllAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _categoryRepository.FindAllAsync();
        }
    }
}
