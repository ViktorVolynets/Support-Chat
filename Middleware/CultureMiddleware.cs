using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Middleware
{
    public class CultureMiddleware
    {

        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            var language = context.Request.Query["lang"].ToString();

            if (language == "") language = "ru-RU";

            try
            {
                CultureInfo.CurrentCulture = new CultureInfo(language);
            }
            catch(CultureNotFoundException ex)
            {

            }

            await _next.Invoke(context);
        }
    }

    //Called from startup "UseCulture"
    public static class CultureExtensions
    {

        public static IApplicationBuilder UseCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CultureMiddleware>();
        }
    }
}
