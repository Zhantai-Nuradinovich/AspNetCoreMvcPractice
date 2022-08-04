using AspNetCoreMvcPractice.Data.Models2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> TakeAsync(int count);
    }
}
