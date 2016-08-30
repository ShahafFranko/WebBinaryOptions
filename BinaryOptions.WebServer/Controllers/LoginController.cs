using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using BinaryOptions.WebServer.Models;

namespace BinaryOptions.WebServer.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("~/Views/Index/Index.cshtml");
            }
            
            return View("~/Views/Login/Login.cshtml");
        }

        [System.Web.Mvc.HttpPost]
        public void Login([FromBody] LoginModel model)
        {
            //Validation code

            if (true)
            {
                FormsAuthentication.SetAuthCookie(model.Username, false);
            }
        }

        [System.Web.Mvc.Authorize]
        [System.Web.Mvc.HttpPost]
        public void LogOut()
        {
            FormsAuthentication.SignOut();
            //Response.Redirect("~/Views/Login/Index.cshtml");
        }
    }
}