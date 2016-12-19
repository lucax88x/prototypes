using System.Web.Http;
using Owin;

namespace IssuingService
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            appBuilder.UseWebApi(config);
        }
    }
}