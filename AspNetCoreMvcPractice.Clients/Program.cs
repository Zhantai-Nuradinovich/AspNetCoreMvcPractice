using AspNetCoreMvcPractice.Clients.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Clients
{
    class Program
    {
        static async Task Main()
        {
            using (var client = new HttpClient())
                await GetAsync(client);
        }

        static async Task GetAsync(HttpClient client)
        {
            client.BaseAddress = new Uri("http://localhost:29247/");
            try
            {
                var categories = await GetListAsync<Category>(client, "api/category");
                var products = await GetListAsync<Product>(client, "api/product");

                foreach (var category in categories)
                    ShowCategory(category);

                foreach (var product in products)
                    ShowProduct(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task<IEnumerable<T>> GetListAsync<T>(HttpClient client, string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var listAsString = await response.Content.ReadAsStringAsync();
            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<T>>(listAsString);
            return list;
        }

        static void ShowProduct(Product product)
        {
            Console.WriteLine(
                $"------------- Product:     {product.ProductName} ------------------------\n" +
                $"      Quantity per unit:   {product.QuantityPerUnit}\n" +
                $"      Price:               {product.UnitPrice}\n" +
                $"      Category:            {product.Category.CategoryName}\n");
        }

        static void ShowCategory(Category category)
        {
            Console.WriteLine($"Category:   {category.CategoryName}\n" +
                            $"              {category.Description,10}\n");
        } 
    }
}
