using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace PrinterMonitoringApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApiWithExtensions",
                routeTemplate: "api/{controller}.{ext}/{action}",
                defaults: new { ext = "json", action = "Get", showHelp = true }
            );

            config.Formatters.JsonFormatter.MediaTypeMappings.Add(new UriPathExtensionMapping("json", "application/json"));
            config.Formatters.XmlFormatter.MediaTypeMappings.Add(new UriPathExtensionMapping("xml", "application/xml"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { ext = "json", action = "Get", showHelp = false }
            );

            config.Formatters.JsonFormatter.AddQueryStringMapping("responseContentType", "json", "application/json");
            config.Formatters.XmlFormatter.AddQueryStringMapping("responseContentType", "xml", "application/xml");


            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }
    }
}
