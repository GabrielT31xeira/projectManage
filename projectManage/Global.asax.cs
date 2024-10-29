using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace projectManage
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var formsIdentity = (FormsIdentity)HttpContext.Current.User.Identity;
                    var authTicket = formsIdentity.Ticket;

                    // Recuperar os perfis do ticket de autenticação
                    string[] roles = authTicket.UserData.Split(',');

                    // Configurar o principal com os perfis de usuário
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(formsIdentity, roles);
                }
            }
        }
    }
}