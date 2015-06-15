using System;
using System.Net.Http;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.DependencyInjection;

namespace ContactManager
{
    public class LinkProvider
    {
        public virtual Uri GetLink(HttpContext requestContext, string routeName, object routeParams)
        {
            var urlHelper = requestContext.RequestServices.GetRequiredService<IUrlHelper>();
            return new Uri(urlHelper.Link(routeName, routeParams), UriKind.Absolute);
        }
    }
}