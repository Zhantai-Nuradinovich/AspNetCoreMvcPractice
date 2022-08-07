using AspNetCoreMvcPractice.Data.Interfaces;
using AspNetCoreMvcPractice.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(DbContext dbContext) : base(dbContext) { }
        public override async Task<Product> FindAsync(Expression<Func<Product, bool>> predicate)
        {
            return await Context.Set<Product>()
                .Include(s => s.Supplier)
                .Include(c => c.Category)
                .AsNoTracking()
                .FirstAsync(predicate);
        }

        public override async Task<IEnumerable<Product>> FindAllAsync()
        {
            return await Context.Set<Product>()
               .Include(s => s.Supplier)
               .Include(c => c.Category)
               .OrderByDescending(p => p.ProductID)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<Product>> TakeAsync(int count)
        {
            return await Context.Set<Product>()
                .Include(s => s.Supplier)
                .Include(c => c.Category)
                .OrderByDescending(p => p.ProductID)
                .Take(count)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
