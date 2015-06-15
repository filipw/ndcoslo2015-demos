1. Copy just controller with `Get`
2. Change `RoutePrefix` to `Route("[controller]")`
3. Add singleton `InMemoryContactRepostiory`
4. Add XML in `AddMvc` => opt => `AddXmlDataContractSerializerFormatter`
5. Copy remaining actions and rename `IHttpActionResult` to `IActionResult`
6. Change `[Route]` to `[Http*]`
7. Register `IContentNegotiator`

```
services.AddInstance<IContentNegotiator>(new DefaultContentNegotiator());

opt.OutputFormatters.Insert(0, new HttpResponseMessageOutputFormatter());

Configure<WebApiCompatShimOptions>(opt =>
            {
                opt.Formatters.AddRange(new MediaTypeFormatterCollection());
            });
```
8. Show POST failure
9. Use `[FromBody]` and show `FromBodyConvention`
10. Copy `ContactSelfLinkFilter` and `LinkProvider`

```
     public virtual Uri GetLink(HttpContext context, string routeName, object routeParams)
        {
            var urlHelper = context.RequestServices.GetRequiredService<IUrlHelper>();
            return new Uri(urlHelper.Link(routeName, routeParams), UriKind.Absolute);
        }
```

```
    public class ContactSelfLinkFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            var response = actionExecutedContext.Result as ObjectResult;

            Contact contact;
            if (response.TryGetContentValue(out contact))
            {
                AddSelfLink(contact, actionExecutedContext);
                return;
            }

            IEnumerable<Contact> contacts;
            if (response.TryGetContentValue(out contacts))
            {
                response.Value = contacts.Select(c => AddSelfLink(c, actionExecutedContext));
            }
        }

        static Contact AddSelfLink(Contact contact, ActionExecutedContext context)
        {
            //var linkProvider = context.Request.GetDependencyScope().GetService(typeof(LinkProvider)) as LinkProvider;
            var linkProvider = new LinkProvider();
            if (linkProvider == null)
            {
                throw new Exception("Link provider not found");
            }

            contact.Self = linkProvider.GetLink(context.HttpContext, "GetContactById", new { id = contact.ContactId }).ToString(); ;
            return contact;
        }
    }
```
9. Change `LinkProvider` to constructor injection (and register in DI)
10. Change `[ContactSelfLinkFilter]` to `[TypeFilter(typeof(ContactSelfLinkFilter))]`
11. Copy `TimerHandler` into `TimerMiddleware`

```
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
```
12. Register `app.UseMiddleware<TimerMiddleware>();`
13. Show 406 in Web API, add `HttpNotAcceptableOutputFormatter` in `Startup`
14. Show `?format=json` in Web API and add `FormatFilter`
15. Show `ErrorHandler` and move to MVC 6. Remember to comment out `BufferingMiddleware`!
```
app.UseErrorHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.GetFeature<IErrorHandlerFeature>();
                    var metadata = new ErrorData
                    {
                        Message = "An unexpected error occurred! The error ID will be helpful to debug the problem",
                        DateTime = DateTimeOffset.Now,
                        RequestUri = new Uri(context.Request.Host.ToString() + context.Request.Path.ToString() + context.Request.QueryString),
                        ErrorId = Guid.NewGuid()
                    };

                    if (context.Connection.IsLocal)
                    {
                        if (errorFeature.Error != null)
                        {
                            metadata.Exception = errorFeature.Error;
                        }
                    }

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(metadata));
                });
            });
```
16. Show `CsvMediaTypeFormatter` in action and move to MVC 6, method by method. Remember to add supported media types in constructor

```
var itemType = context.DeclaredType.GetElementType() ?? context.DeclaredType.GetGenericArguments()[0];

var writer = new StreamWriter(context.HttpContext.Response.Body);
```
17. Show `CsvOnlyController`, copy to MVC6 and change `[CsvOnly]` to `[Produces("text/csv")]`
