using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace ContactManager.Middleware
{
    public class BufferingMiddleware
    {
        private readonly RequestDelegate _next;

        public BufferingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext ctx)
        {
            var networkStream = ctx.Response.Body;
            var bufferedStream = new MemoryStream();
            ctx.Response.Body = bufferedStream;

            await _next(ctx);

            bufferedStream.Position = 0;
            ctx.Response.ContentLength = bufferedStream.Length;
            await bufferedStream.CopyToAsync(networkStream);
        }
    }
}