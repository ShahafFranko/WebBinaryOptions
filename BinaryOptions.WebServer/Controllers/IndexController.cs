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
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> OpenPosition([FromBody] OpenPositionModel model)
        {
            // First lets create path to our handler.
            string openPositionCommandHandler = Global.Protocol.GenerateTcpPath("OpenPositionCommandHandler");
            
            // now let's send account creation request.
            var request = new OpenPositionRequest(model.Username,model.Direction,model.InstrumentName,model.Amount);
            var response = await Global.ActorSystem.ActorSelection(openPositionCommandHandler).Ask<IReply>(request) as OpenPositionReply;

            return Json(response);
        }
    }
}