using AspNetCoreMvcPractice.Data.Interfaces;
using AspNetCoreMvcPractice.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.FindAllAsync();
        }

        public async Task<byte[]> GetPictureByIdAsync(int id)
        {
            var category = await GetByIdAsync(id);
            return category.Picture;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var model = await _categoryRepository.FindAsync(p => p.CategoryID == id);
            if (model == null)
                throw new InvalidOperationException($"Category with Id: {id} not found");

            return model;
        }

        public async Task EditImageById(int categoryID, byte[] picture)
        {
            var model = await GetByIdAsync(categoryID);
            model.Picture = picture;
            await _categoryRepository.UpdateAsync(model);
        }
    }
}
