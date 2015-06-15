using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace ContactManager.Middleware
{
    public class TimerMiddleware
    {
        private readonly RequestDelegate _next;

        public TimerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await _next(context);

            stopWatch.Stop();
            context.Response.Headers.Add("ExecutionTime", new [] { stopWatch.ElapsedMilliseconds.ToString() });
        }
    }
}
