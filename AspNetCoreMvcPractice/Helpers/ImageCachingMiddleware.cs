using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Helpers
{
    public class ImageCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public ImageCacheMiddleware(RequestDelegate next,
                                    IMemoryCache cache,
                                    IWebHostEnvironment env,
                                    IConfiguration configuration)
        {
            _next = next;
            _cache = cache;
            _env = env;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!IsCashableAction(context))
            {
                await _next(context);
                return;
            }

            var key = GetKey(context);
            _cache.TryGetValue(key, out string path);

            if (path == null)
            {
                var imagePath = GetImagePath(context);
                DeleteFileIfExists(imagePath);
                await ExecuteRequestAndCache(context, key, imagePath);
            }
            else
            {
                _cache.TryGetValue(GetContentTypeKey(key), out string contentType);
                await SetResponseFromCache(context, path, contentType);
            }
        }

        private string GetImagePath(HttpContext context)
        {
            var id = context.Request.Path.Value.Substring(8);
            var imagesPath = Path.Combine(_env.WebRootPath, "CachedImages");
            return Path.Combine(imagesPath, id);
        }

        private string GetKey(HttpContext context)
        {
            var path = Encoding.UTF8.GetBytes(context.Request.Path);
            var hash = GetHash(path);

            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private byte[] GetHash(byte[] uniqueData)
        {
            var sha256 = SHA256.Create();
            return sha256.ComputeHash(uniqueData);
        }

        private bool IsCashableAction(HttpContext context)
        {
            //hardcode
            return context.Request.Path.Value.Contains("Images");
        }

        private async Task ExecuteRequestAndCache(HttpContext context, string key, string imagePath)
        {
            var originalBodyStream = context.Response.Body;
            var minutesToStore = _configuration.GetValue<int>("Caching:ExpirationTimeInMinutes");

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var buffer = new byte[Convert.ToInt32(context.Response.Body.Length)];
                context.Response.Body.Read(buffer, 0, buffer.Length);

                _cache.Set(key, imagePath, new TimeSpan(0, minutesToStore, 0));
                _cache.Set(GetContentTypeKey(key), context.Response.ContentType, new TimeSpan(0, minutesToStore, 0));
                await CreateFileAsync(imagePath, buffer);

                context.Response.Body.Seek(0, SeekOrigin.Begin);

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        private string GetContentTypeKey(string key)
        {
            return $"{key}ContentType";
        }
        private async Task SetResponseFromCache(HttpContext context, string path, string contentType)
        {
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                context.Response.OnStarting(state =>
                {
                    var httpContext = (HttpContext)state;
                    httpContext.Response.Headers.Add("Content-Type", contentType);
                    return Task.CompletedTask;
                }, context);
            }

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var bytes = File.ReadAllBytes(path);
                context.Response.Body.Write(bytes, 0, bytes.Length);
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        private async Task CreateFileAsync(string imagePath, byte[] buffer)
        {
            using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                await stream.WriteAsync(buffer);
        }
        private void DeleteFileIfExists(string path)
        {
            var isExists = File.Exists(path);
            if (isExists)
                File.Delete(path);
        }
    }
}
