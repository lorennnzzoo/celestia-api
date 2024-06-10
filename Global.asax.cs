using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace celestia_api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Check if the user already has the cookie
            if (HttpContext.Current.Request.Cookies["user_cookie"] == null)
            {
                // Generate a new GUID
                string guidValue = Guid.NewGuid().ToString();

                // Set the cookie here
                HttpCookie cookie = new HttpCookie("user_cookie", guidValue);

                // Make the cookie permanent by setting its expiration to a distant future date
                cookie.Expires = DateTime.Now.AddYears(50); // Or any other suitable distant future date

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}
