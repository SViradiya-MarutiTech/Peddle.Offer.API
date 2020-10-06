using Microsoft.AspNetCore.Builder;
using Peddle.Offer.Api.Middlewares;

namespace Peddle.Offer.Api.Extensions
{
    public static class AppExtensions
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
