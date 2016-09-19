using BinaryOptions.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.DAL.Repositories
{
    public class InstrumentRateRepository
    {
        private Dictionary<Guid, List<InstrumentRate>> m_repo = new Dictionary<Guid, List<InstrumentRate>>();

        public InstrumentRateRepository()
        {
        }

        public void UpdateRate(InstrumentRate instrumentRate)
        {
            if (!m_repo.ContainsKey(instrumentRate.Id))
            {
                m_repo.Add(instrumentRate.Id, new List<InstrumentRate>());
            }
            m_repo[instrumentRate.Id].Add(instrumentRate);
        }

        public IList<InstrumentRate> GetInstrumentById(Guid instrumentId) 
        {
            return m_repo[instrumentId];
        }

    }

}
