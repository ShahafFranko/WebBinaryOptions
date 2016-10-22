using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryOptions.DAL.Data;
using BinaryOptions.DAL;

namespace BinaryOption.DAL.Repositories
{
    public class InstrumentRepository
    {
        private IDictionary<Guid, Instrument> m_instruments;

        public InstrumentRepository()
        {
            using(var ctx = BinaryOptionsContext.Create())
            {
                m_instruments = ctx.Instruments.Where(i => i.IsEnabled).ToDictionary(item => item.Id, item => item);
            }
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
                using (var ctx = BinaryOptionsContext.Create())
                {
                    Instrument instrumentToUpdate = ctx.Instruments.Find(instrument.Id);
                    // Update local cache.
                    m_instruments[instrument.Id] = instrument;
                    
                    // Update db.
                    instrumentToUpdate.Rate = instrument.Rate;
                    ctx.SaveChanges();
                }
                                
            }
        }
    }
}
