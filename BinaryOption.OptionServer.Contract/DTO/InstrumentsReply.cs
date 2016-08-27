using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.DTO
{
    public class InstrumentsReply : IReply
    {
        public InstrumentsReply(List<string> instruments)
        {
            Instruments = instruments;
        }

        public List<string> Instruments { get; private set; }
    }
}
