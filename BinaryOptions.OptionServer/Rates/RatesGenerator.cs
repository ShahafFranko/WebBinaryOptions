using BinaryOptions.OptionServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOptions.OptionServer.Rates
{
    public class RatesGenerator
    {
        private IList<Instrument> m_instruments; 
        public RatesGenerator()
        {
            m_instruments = new List<Instrument>();
            m_instruments.Add(new Instrument("EURUSD", 1.0, 2.0));
            m_instruments.Add(new Instrument("EURGBP", 1.1, 2.5));
            m_instruments.Add(new Instrument("GBPUSD", 1.2, 2.4));
            m_instruments.Add(new Instrument("USDJPY", 14.1, 23.23));
        }

        public void Generate() 
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
