using System;
using System.Net.Http;

namespace ContactsManager.Filters
{
    public class LinkProvider
    {
        public virtual Uri GetLink(HttpRequestMessage request, string routeName, object routeParams)
        {
            var urlHelper = request.GetUrlHelper();
            return new Uri(urlHelper.Route(routeName, routeParams), UriKind.Relative);
        }
    }
}