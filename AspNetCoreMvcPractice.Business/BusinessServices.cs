using AspNetCoreMvcPractice.Business.Services;
using AspNetCoreMvcPractice.Data;
using AspNetCoreMvcPractice.Data.Interfaces;
using AspNetCoreMvcPractice.Data.Models;
using AspNetCoreMvcPractice.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMvcPractice.Business
{
    public static class BusinessServices
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NorthwindDbContext>(options =>
            {
                var connection = configuration.GetValue<string>("ConnectionStrings:NorthwindDbContext");
                options.UseSqlServer(connection);
            });

            services.AddScoped<ICategoryService, CategoryService>()
                    .AddScoped<IProductService, ProductService>();

            services.AddScoped<IProductRepository, ProductRepository>()
                    .AddScoped<IRepository<Category>, GenericRepository<Category>>()
                    .AddScoped<IRepository<Supplier>, GenericRepository<Supplier>>();

            services.AddScoped<DbContext, NorthwindDbContext>();

            services.AddIdentity<User, UserRole>()
                    .AddEntityFrameworkStores<NorthwindDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
