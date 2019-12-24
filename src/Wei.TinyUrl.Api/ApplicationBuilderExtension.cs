using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wei.TinyUrl.Data;
using Wei.TinyUrl.Data.Entities;
using Wei.TinyUrl.Data.Repositories;

namespace Wei.TinyUrl.Api
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseTinyUrl(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UrlMappingMiddleWare>();
        }

        public static void LoadClients(this IApplicationBuilder builder)
        {
            var cache = (IMemoryCache)builder.ApplicationServices.GetService(typeof(IMemoryCache));
            var clients = JsonConfigurationHelper.GetAppSettings<List<Client>>("Clients");
            if (clients.HasValue()) clients.ForEach(x => cache.Set(x.Key, x));
        }
    }

    public class UrlMappingMiddleWare
    {
        private readonly RequestDelegate _next;
        public UrlMappingMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            if (path.HasValue)
            {
                var pathValue = path.Value;
                if (pathValue.StartsWith("/")) pathValue = pathValue.Substring(1);
                if (Regex.IsMatch(pathValue, @"^[a-zA-Z0-9]{6}$"))
                {
                    var urlMappingRepository = (IUrlMappingRepository)context.RequestServices.GetService(typeof(IUrlMappingRepository));
                    var longUrl = await urlMappingRepository.GetUrlByCode(pathValue);
                    if (!string.IsNullOrEmpty(longUrl))
                    {
                        if (!Regex.IsMatch(longUrl, "(file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://", RegexOptions.IgnoreCase))
                        {
                            longUrl = "http://" + longUrl;
                        }
                        context.Response.Redirect(longUrl, true);
                        return;
                    }
                }
            }
            await _next.Invoke(context);
        }
    }
}
