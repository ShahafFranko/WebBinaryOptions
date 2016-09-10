using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Akka.Actor;
using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Requests;
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
        public async Task<Guid> Login([FromBody] LoginModel model)
        {
            LoginRequest request = new LoginRequest(model.Username, model.Password);
            string loginHandler = Global.Protocol.GenerateTcpPath("LoginRequestHandler");
            LoginReply response = await Global.ActorSystem.ActorSelection(loginHandler).Ask<LoginReply>(request);
            
            if (response.SuccessfulLogin)
            {
                FormsAuthentication.SetAuthCookie(model.Username, false);
            }

            return response.AccountId;
        }

        [System.Web.Mvc.Authorize]
        [System.Web.Mvc.HttpPost]
        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}