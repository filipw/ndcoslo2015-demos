using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using ContactManager.Filters;
using ContactManager.Formatters;
using ContactManager.Middleware;
using ContactManager.Models;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.WebApiCompatShim;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.WebEncoders;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace ContactManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IContactRepository, InMemoryContactRepository>();
            services.AddInstance<IContentNegotiator>(new DefaultContentNegotiator(excludeMatchOnTypeOnly: true));
            services.AddSingleton<LinkProvider>();
            services.AddSingleton<ContactSelfLinkFilter>();

            services.AddMvc().Configure<MvcOptions>(options =>
            {
                options.OutputFormatters.Insert(0, new HttpResponseMessageOutputFormatter());
                options.OutputFormatters.Insert(0, new HttpNotAcceptableOutputFormatter());
                options.OutputFormatters.Insert(0, new CsvMediaTypeFormatter());
                options.AddXmlDataContractSerializerFormatter();
                options.RespectBrowserAcceptHeader = true;

                options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
            });

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
           //services.AddWebApiConventions();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

                    var connection = context.GetFeature<IHttpConnectionFeature>();
                    if (connection.IsLocal)
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

            //app.UseMiddleware<BufferingMiddleware>();
            app.UseMiddleware<TimerMiddleware>();

            // Configure the HTTP request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc();
            // Add the following route for porting Web API 2 controllers.
            // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
        }
    }
}
