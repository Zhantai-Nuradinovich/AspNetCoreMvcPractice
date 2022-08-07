using AspNetCoreMvcPractice.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Business.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<byte[]> GetPictureByIdAsync(int id);
        Task<Category> GetByIdAsync(int id);
        Task EditImageById(int categoryID, byte[] picture);
    }
}
