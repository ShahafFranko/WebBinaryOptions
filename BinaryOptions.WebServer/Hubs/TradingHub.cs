using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Akka.Actor;
using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Events;
using BinaryOption.OptionServer.Contract.Requests;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BinaryOptions.WebServer.Hubs
{
    [HubName("tradingHub")]
    public class TradingHub : Hub
    {
        public void Subscribe(Guid accountId)
        {
        }

        public void PushAccount(AccountUpdated @event)
        {
            Clients.All.onAccountUpdated(@event);
        }

        public void PushInstrument(InstrumentUpdated @event)
        {
            Clients.All.onInstrumentUpdated(@event);
        }

        public async Task<IList<string>> GetInstruments()
        {
            string ratesActorPath = Global.Protocol.GenerateTcpPath("RatesService");
            IReply reply = await Global.ActorSystem.ActorSelection(ratesActorPath).Ask<IReply>(new InstrumentsRequest());

            var response = reply as InstrumentsReply;

            if (response == null)
            {
                Clients.Caller.onError("Failed to retrieve instrument list.");
                return null;
            }

            return response.Instruments;
        }
    }
}