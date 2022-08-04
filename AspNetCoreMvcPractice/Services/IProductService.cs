using AspNetCoreMvcPractice.Data.Models2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Services
{
    public interface IProductService
    {
        Task CreateAsync(Product Product);
        Task UpdateAsync(Product Product);
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetMaxAmountAsync(int maxAmountOfProducts);
        Task DeleteAsync(Product Product);
        Task<IEnumerable<Supplier>> GetSuppliers();
        Task<IEnumerable<Category>> GetCategories();
    }
}
