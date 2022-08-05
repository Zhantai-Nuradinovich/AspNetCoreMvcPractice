using AspNetCoreMvcPractice.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Business.Services
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
