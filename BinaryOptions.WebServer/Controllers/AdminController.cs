using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Akka.Actor;
using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Requests;
using BinaryOptions.WebServer.Models;

namespace BinaryOptions.WebServer.Controllers
{
    [System.Web.Mvc.Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View("~/Views/Admin/Admin.cshtml");
        }

        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> Create([FromBody] CreateAccountModel model)
        {
            // First lets create path to our handler.
            string accountsHandlerPath = Global.Protocol.GenerateTcpPath("AccountsHandler");
            
            // now let's send account creation request.
            var request = new CreateAccountRequest(model.Username, model.Password, model.Balance);
            var response = await Global.ActorSystem.ActorSelection(accountsHandlerPath).Ask<IReply>(request) as AccountReply;

            return Json(AccountModel.FromDto(response));
        }

        [System.Web.Mvc.HttpGet]
        public async Task<JsonResult> Accounts()
        {
            // First lets create path to our handler.
            string accountsHandlerPath = Global.Protocol.GenerateTcpPath("AccountsHandler");

            // now let's send account creation request.
            var request = new GetAccountRequest();
            var response = await Global.ActorSystem.ActorSelection(accountsHandlerPath).Ask<IEnumerable<AccountReply>>(request);

            return Json(response.Select(AccountModel.FromDto), JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public async Task<JsonResult> Search([FromUri]PositionsSearchModel searchModel)
        {
            // First lets create path to our handler.
            string searchHandlerPath = Global.Protocol.GenerateTcpPath("SearchRequestsHandler");

            // now let's send account creation request.
            var request = new PositionsSearchRequest(searchModel.OpenTime, searchModel.ExpireTime, searchModel.Descending, searchModel.Wins);
            var response = await Global.ActorSystem.ActorSelection(searchHandlerPath).Ask<IList<PositionDto>>(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}