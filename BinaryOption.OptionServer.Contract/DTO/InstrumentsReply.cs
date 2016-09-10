using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.DTO
{
    public class InstrumentsReply : IReply
    {
        public InstrumentsReply(IList<string> instruments)
        {
            Instruments = instruments;
        }

        public IList<string> Instruments { get; private set; }
    }
}
