using Hotel.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Hotel.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {


        private string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] Roles)
        {
            this.allowedroles = (string[])Roles.Clone();
        }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {


            IserviceUser su = new ServiceUser();

            IPrincipal user = httpContext.User;
            bool authorize = false;



            string userid = user.Identity.Name;
            User _user = su.Get(x => x.mail == userid);


            if (_user != null && Roles.Contains(_user.type))
            {
                authorize = true;
            }





            return authorize;
        }




        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    filterContext.Result = new ViewResult()
            //    {
            //        ViewName = "~/Home/Unauthorized"
            //    };
            //}

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new
                 RouteValueDictionary(new { controller = "User", action = "login" }));
            }
            // filterContext.Result = new HttpUnauthorizedResult();

            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                 RouteValueDictionary(new { controller = "Home", action = "Unauthorized" }));
            }
        }


    }
}