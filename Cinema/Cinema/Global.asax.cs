using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Cinema
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteTable.Routes.MapPageRoute("Films", "Films/{title}", "~/Films/Film.aspx");
            RouteTable.Routes.MapPageRoute("Shows", "Shows/{date}/{time}/{id}", "~/Shows/Book.aspx");
            MySQLServer.connection.OpenAsync();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie =
                          Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket =
                                            FormsAuthentication.Decrypt(authCookie.Value);
                string[] roles = authTicket.UserData.Split(new Char[] { ',' });
                GenericPrincipal userPrincipal =
                                 new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);
                Context.User = userPrincipal;
            }
        }
    }
}
