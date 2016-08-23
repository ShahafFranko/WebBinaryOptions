using System;
using System.Collections.Generic;
using Akka.Actor;
using BinaryOption.OptionServer.Contract.Events;
using BinaryOptions.OptionServer.Entities;

namespace BinaryOptions.OptionServer.Services
{
    public class RatesService : ReceiveActor
    {
        private IList<Instrument> m_instruments;
        
        public RatesService()
        {
            m_instruments = new List<Instrument>();
            m_instruments.Add(new Instrument("EURUSD", 1.0, 2.0));
            m_instruments.Add(new Instrument("EURGBP", 1.1, 2.5));
            m_instruments.Add(new Instrument("GBPUSD", 1.2, 2.4));
            m_instruments.Add(new Instrument("USDJPY", 14.1, 23.23));

            Receive<TenSecondsElapsed>(e => Handle(e));
        }

        private void Handle(TenSecondsElapsed @event)
        {
            // first let's generate fake rates to all instruments.
            GenerateFakeRates();

            // now lets publish those fake rates to our Rates Subscriber.
            var ratesSubscriber = Context.ActorSelection("BinaryOptions.UI/RatesSubscriber");
            
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
                double d = rand.NextDouble() * (instrument.Max - instrument.Min) + instrument.Min;
                instrument.Update(d);
            }
        }
    }
}
