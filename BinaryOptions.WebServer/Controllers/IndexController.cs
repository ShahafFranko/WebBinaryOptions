using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Requests;
using BinaryOptions.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Akka.Actor;

namespace BinaryOptions.WebServer.Controllers
{
    [System.Web.Mvc.Authorize]
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.Name == "admin")
            {
                return View("~/Views/Admin/Admin.cshtml");
            }

            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> OpenPosition([FromBody] PositionModel model)
        {
            // First lets create path to our handler.
            string openPositionCommandHandler = Global.Protocol.GenerateTcpPath("OpenPositionCommandHandler");
            
            // now let's send account creation request.
            var request = new OpenPositionRequest(model.AccountId, model.Direction, model.InstrumentName, model.Amount);
            var response = await Global.ActorSystem.ActorSelection(openPositionCommandHandler).Ask<IReply>(request) as OpenPositionReply;

            return Json(response);
        }
        
        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> GetAccount([FromBody] Guid accountId)
        {
            // First lets create path to our handler.
            string accountsHandler = Global.Protocol.GenerateTcpPath("AccountsHandler");
            
            // now let's send account creation request.
            var request = new AccountRequest(accountId);
            var response = await Global.ActorSystem.ActorSelection(accountsHandler).Ask<IReply>(request) as AccountReply;

            return Json(response);
        }

        [System.Web.Mvc.HttpGet]
        public async Task<JsonResult> GetInstrumentRates([FromUri] Guid instrumentId)
        {
            // First lets create path to our handler.
            string ratesService = Global.Protocol.GenerateTcpPath("RatesService");

            // now let's send account creation request.
            var request = new InstrumentRatesRequest(instrumentId);
            var response = await Global.ActorSystem.ActorSelection(ratesService).Ask<IReply>(request) as InstrumentRatesReply;

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}