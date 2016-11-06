using System;
using Microsoft.AspNet.SignalR;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Owin;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing.Conventions;
using System.Web.Routing;
using Mn.NewsCms.Common;
using Mn.NewsCms.Web.Controllers;

namespace Mn.NewsCms.Web
{
    public class OwinAppStartup
    {
        public void Configuration(IAppBuilder owinApp)
        {
            //owinApp.Map("/odata", innerOwinAppForOData =>
            //{
            //    HttpConfiguration webApiODataConfig = new HttpConfiguration();
            //    webApiODataConfig.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            //    webApiODataConfig.Formatters.Clear();

            //    IEnumerable<IODataRoutingConvention> conventions = ODataRoutingConventions.CreateDefault();

            //    ODataModelBuilder modelBuilder = new ODataConventionModelBuilder(webApiODataConfig);

            //    modelBuilder.Namespace = modelBuilder.ContainerName = "Test";
            //    var categoriesSetConfig = modelBuilder.EntitySet<Category>("Categories");
            //    var getBestCategoryFunctionConfig = categoriesSetConfig.EntityType.Collection.Function(nameof(CatController.Index));
            //    getBestCategoryFunctionConfig.ReturnsFromEntitySet<Category>("Categories");

            //    IEdmModel edmModel = modelBuilder.GetEdmModel();

            //    webApiODataConfig.MapODataServiceRoute("default", "", builder =>
            //    {
            //        builder.AddService(ServiceLifetime.Singleton, sp => conventions);
            //        builder.AddService(ServiceLifetime.Singleton, sp => edmModel);
            //    });

            //    innerOwinAppForOData.UseWebApi(webApiODataConfig);

            //});         

            owinApp.Map("/api", innerOwinAppForWebApi =>
            {
                HttpConfiguration webApiConfig = new HttpConfiguration();
                webApiConfig.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

                webApiConfig.MapHttpAttributeRoutes();

                webApiConfig.Routes.MapHttpRoute(name: "default", routeTemplate: "{controller}/{action}", defaults: new { action = RouteParameter.Optional });

                innerOwinAppForWebApi.UseWebApi(webApiConfig);
            });

            owinApp.Map("/tag/all", innerOwinAppForWebApi =>
            {
                HttpConfiguration webApiConfig = new HttpConfiguration();
                webApiConfig.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

                webApiConfig.MapHttpAttributeRoutes();

                webApiConfig.Routes.MapHttpRoute(name: "all", routeTemplate: "{controller}/{action}", defaults: new { action = RouteParameter.Optional });

                innerOwinAppForWebApi.UseWebApi(webApiConfig);
            });

            owinApp.Map("/signalr", innerOwinAppForSignalR =>
            {
                innerOwinAppForSignalR.RunSignalR(new HubConfiguration
                {
                    EnableDetailedErrors = true
                });
            });

            owinApp.UseStaticFiles();

            owinApp.Run(async context =>
            {
                await context.Response.WriteAsync("owin katana");
            });

            Bootstrapper.Initialise();
        }
    }
}