using Microsoft.AspNetCore.Builder;
using Ms.Arch.Hw02.Api.CommonCore.CommonMiddleware;

namespace Ms.Arch.Hw02.Api.CommonCore
{
    public static class CommonExtensions
    {
        public static void UseCommonVersion(this IApplicationBuilder app)
        {
            app.UseCommonMiddleware<CommonVersionMiddleware>("/version");
        }

        private static void UseCommonMiddleware<TMiddleware>(this IApplicationBuilder app, string path)
        {
            app.Map(path, b => b.UseMiddleware<TMiddleware>());
        }
    }
}
