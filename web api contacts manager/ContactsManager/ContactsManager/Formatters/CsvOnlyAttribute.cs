using System;
using System.Web.Http.Controllers;

namespace ContactsManager.Formatters
{
    public class CsvOnlyAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings,
            HttpControllerDescriptor controllerDescriptor)
        {
            controllerSettings.Formatters.Clear();
            controllerSettings.Formatters.Add(new CsvMediaTypeFormatter());
        }
    }
}