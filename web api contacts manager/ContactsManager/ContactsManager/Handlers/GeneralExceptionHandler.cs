using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Newtonsoft.Json;

namespace ContactsManager.Handlers
{
    public class GeneralExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var metadata = new ErrorData
            {
                Message = "An unexpected error occurred! The error ID will be helpful to debug the problem",
                DateTime = DateTimeOffset.Now,
                RequestUri = context.Request.RequestUri,
                ErrorId = Guid.NewGuid()
            };

            if (context.Request.IsLocal())
            {
                if (context.Exception != null)
                {
                    metadata.Exception = context.Exception;
                }
            }

            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            response.Content = new StringContent(JsonConvert.SerializeObject(metadata));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            context.Result = new ResponseMessageResult(response);
        }
    }
}