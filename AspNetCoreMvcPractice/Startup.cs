using AspNetCoreMvcPractice.Data;
using AspNetCoreMvcPractice.Data.Interfaces;
using AspNetCoreMvcPractice.Data.Models;
using AspNetCoreMvcPractice.Data.Repositories;
using AspNetCoreMvcPractice.Helpers;
using AspNetCoreMvcPractice.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<NorthwindDbContext>(options => 
            {
                var connection = Configuration.GetValue<string>("ConnectionStrings:NorthwindDbContext");
                options.UseSqlServer(connection);
            });

            services.AddScoped<ICategoryService, CategoryService>()
                    .AddScoped<IProductService, ProductService>();

            services.AddScoped<IProductRepository, ProductRepository>()
                    .AddScoped<IRepository<Category>, GenericRepository<Category>>()
                    .AddScoped<IRepository<Supplier>, GenericRepository<Supplier>>();

            services.AddScoped<DbContext, NorthwindDbContext>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Configuration.GetSection("Logging"));

            var logger = loggerFactory.CreateLogger<Startup>();
            foreach (var config in Configuration.GetChildren())
                logger.LogInformation($"{config.Key} ::: {config.Value}");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
