using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using BinaryOption.OptionServer.Contract;
using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Events;
using BinaryOption.OptionServer.Contract.Requests;
using BinaryOptions.DAL.Data;

namespace BinaryOptions.OptionServer.Services
{
    public class RatesService : ReceiveActor
    {
        private readonly Protocol m_protocol;
        private IList<Instrument> m_instruments;
        
        public RatesService(Protocol protocol)
        {
            m_protocol = protocol;
            m_instruments = new List<Instrument>();
            m_instruments.Add(new Instrument("EURUSD", 1.0, 2.0));
            m_instruments.Add(new Instrument("EURGBP", 1.1, 2.5));
            m_instruments.Add(new Instrument("GBPUSD", 1.2, 2.4));
            m_instruments.Add(new Instrument("USDJPY", 14.1, 23.23));

            Receive<OneSecondElapsed>(e => Handle(e));
            Receive<InstrumentsRequest>(c => GetInstrumentNames(c));
        }

        private void GetInstrumentNames(InstrumentsRequest instrumentsRequest)
        {
            Sender.Tell(new InstrumentsReply(m_instruments.Select(i => i.Name).ToList()), Self);
        }

        private void Handle(OneSecondElapsed @event)
        {
            // first let's generate fake rates to all instruments.
            GenerateFakeRates();

            // now lets publish those fake rates to our Rates Subscriber.
            var ratesSubscriber = Context.ActorSelection(m_protocol.GenerateTcpPath("EventsListener"));
            
            foreach (Instrument instrument in m_instruments)
            {
                ratesSubscriber.Tell(new InstrumentUpdated(instrument.Name, instrument.Rate), Self);
            }
        }

        private void GenerateFakeRates() 
        {
            Random rand = new Random();

            foreach (var instrument in m_instruments)
            {
                double d = Math.Round(rand.NextDouble() * (instrument.Max - instrument.Min) + instrument.Min, 4);
                instrument.Update(d);
            }
        }
    }
}
