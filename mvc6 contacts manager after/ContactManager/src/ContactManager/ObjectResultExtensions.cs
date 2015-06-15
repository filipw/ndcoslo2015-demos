using System.Net.Http;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;

namespace ContactManager
{
    public static class ObjectResultExtensions
    {
        public static bool TryGetContentValue<T>(this ObjectResult objectResult, out T value) where T : class
        {
            value = default(T);
            var responseMessage = objectResult.Value as HttpResponseMessage;
            var content = responseMessage?.Content as ObjectContent;
            if (content?.Value is T)
            {
                value = (T) content.Value;
                return true;
            }

            var responseObject = objectResult.Value as T;
            if (responseObject != null)
            {
                value = responseObject;
                return true;
            }

            return false;
        }
    }

    //public static class HttpContextExtensions
    //{
    //    public static bool IsLocal(this HttpContext context)
    //    {
    //        context.
    //        if (request.Properties.ContainsKey(OwinContext))
    //        {
    //            dynamic ctx = request.Properties[OwinContext];
    //            if (ctx != null)
    //            {
    //                return ctx.Request.RemoteIpAddress;
    //            }
    //        }
    //    }
    //}
}