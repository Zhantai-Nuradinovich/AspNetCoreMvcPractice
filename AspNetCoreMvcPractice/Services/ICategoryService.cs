using AspNetCoreMvcPractice.Data.Models2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
    }
}
