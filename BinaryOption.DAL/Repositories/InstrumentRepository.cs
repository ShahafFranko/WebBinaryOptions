using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryOptions.DAL.Data;

namespace BinaryOption.DAL.Repositories
{
    public class InstrumentRepository
    {
        private IDictionary<Guid, Instrument> m_instruments;

        public InstrumentRepository()
        {
            m_instruments = new Dictionary<Guid, Instrument>();
            m_instruments.Add(Guid.NewGuid(), new Instrument("EURUSD", 1.0, 2.0, 0.6));
            m_instruments.Add(Guid.NewGuid(), new Instrument("EURGBP", 1.1, 2.5, 0.67));
            m_instruments.Add(Guid.NewGuid(), new Instrument("GBPUSD", 1.2, 2.4, 0.66));
            m_instruments.Add(Guid.NewGuid(), new Instrument("USDJPY", 14.1, 23.23, 0.62));
        }

        public Instrument GetInstrument(Guid instrumentId)
        {
            Instrument instrument;
            m_instruments.TryGetValue(instrumentId, out instrument);

            return instrument;
        }

        public Instrument GetInstrument(string name)
        {
            return m_instruments.Values.SingleOrDefault(i => i.Name == name);
        }

        public IList<string> GetInstrumentsNames()
        {
            return m_instruments.Values.Select(i => i.Name).ToList();
        }

        public IList<Instrument> GetInstruments()
        {
            return m_instruments.Values.ToList();
        }

        public void Update(Instrument instrument)
        {
            if (m_instruments.ContainsKey(instrument.Id))
            {
                m_instruments[instrument.Id] = instrument;                
            }
        }
    }
}
