using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.DTO
{
    public class InstrumentRatesReply : IReply
    {
        public InstrumentRatesReply(IList<InstrumentRateReply> rates)
        {
            Rates = rates;
        }

        public IList<InstrumentRateReply> Rates { get; private set; }
    }

    public class InstrumentRateReply
    {
        public InstrumentRateReply(Guid id, double rate, DateTime time)
        {
            Id = id;
            Rate = rate;
            Time = time;
        }

        public Guid Id { get; private set; }
        public double Rate { get; private set; }
        public DateTime Time { get; private set; }
    }
}
