using System;
using System.Web.Mvc;
using System.Web.Routing;
using PeoplesSource.App;

namespace PeoplesSource
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("elmah.axd");

            routes.MapRoute(
                "AboutUs",
                "AboutUs",
                new { controller = "Home", action = "AboutUs" }
                );

            routes.MapRoute(
                "ContactUs",
                "ContactUs",
                new { controller = "Home", action = "ContactUs" }
                );

            routes.MapRoute(
                "Index",
                "Index",
                new { controller = "Home", action = "Index" }
                );

            routes.MapRoute(
                "Login",
                "Login",
                new { controller = "Account", action = "Login" }
                );

            routes.MapRoute(
                "Services",
                "Services",
                new { controller = "Home", action = "Services" }
                );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "PeoplesSource.Controllers" }
            );

        }

        protected void Application_Start()
        {
            Bootstrapper.ConfigureIoC();
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            RegisterControllerFactory(ControllerBuilder.Current);
            RegisterGlobalFilters(GlobalFilters.Filters);
        }

        public static void RegisterControllerFactory(ControllerBuilder controllerBuilder)
        {
            controllerBuilder.SetControllerFactory(typeof(CustomControllerFactory));
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            //Session.Timeout = Convert.ToInt32(Session.Timeout.Timeout.TotalMinutes);
        }

        //protected void Application_EndRequest()
        //{
        //    var context = new HttpContextWrapper(Context);
        //    // If we're an ajax request, and doing a 302, then we actually need to do a 401

        //    if (!string.IsNullOrEmpty(context.Response.RedirectLocation))
        //    {
        //        if (context.Response.StatusCode == 302 && context.Response.RedirectLocation.Contains(FormsAuthentication.LoginUrl) && context.Request.IsAjaxRequest())
        //        {
        //            context.Response.Clear();
        //            context.Response.StatusCode = 401;
        //        }
        //    }
        //}
    }
}