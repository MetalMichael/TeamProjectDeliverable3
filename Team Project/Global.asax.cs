using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using System.Reflection;


namespace TimetableSystem
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}
    public class RazorViewEngine : VirtualPathProviderViewEngine
    {
        public RazorViewEngine()
        {
            MasterLocationFormats = new[] { "~/Views/{1}/{0}.cshtml" };
            AreaMasterLocationFormats = new[] { "~/Areas/{2}/Views/{1}/{0}.cshtml" };
            PartialViewLocationFormats = ViewLocationFormats = new[] {  
                "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml"};
            AreaPartialViewLocationFormats = AreaViewLocationFormats = new[] {  
                "~/Areas/{2}/Views/{1}/{0}.cshtml",  
                "~/Areas/{2}/Views/Shared/{0}.cshtml"};
        }

        protected override IView CreatePartialView(
            ControllerContext controllerContext,
            string partialPath)
        {
            return new RazorView { ViewPath = partialPath };
        }

        protected override IView CreateView(
            ControllerContext controllerContext,
            string viewPath, string masterPath)
        {
            return new RazorView { ViewPath = viewPath };
        }

        private class RazorView : IView
        {
            public string ViewPath;

            public void Render(ViewContext viewContext, TextWriter writer)
            {
                var pc = new WebPageContext();

                const BindingFlags bindingFlags = BindingFlags.SetProperty
                    | BindingFlags.Instance
                    | BindingFlags.NonPublic;
                typeof(WebPageContext).InvokeMember("HttpContext", bindingFlags,
                    null, pc, new[] { viewContext.HttpContext });
                typeof(WebPageContext).InvokeMember("ViewContext", bindingFlags,
                    null, pc, new[] { viewContext });

                var wb = WebPageBase.CreateInstanceFromVirtualPath(ViewPath);
                wb.ExecutePageHierarchy(pc, writer);
            }
        }
    }