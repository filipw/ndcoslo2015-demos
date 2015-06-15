using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ContactsManager.Handlers
{
    public class TimerHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var stopWatch = new Stopwatch();
            
            stopWatch.Start();
            var response = await base.SendAsync(request, cancellationToken);
            stopWatch.Stop();

            response.Headers.Add("ExecutionTime", stopWatch.ElapsedMilliseconds.ToString());
            return response;
        }
    }
}