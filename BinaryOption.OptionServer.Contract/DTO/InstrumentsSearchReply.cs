using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.DTO
{
    public class InstrumentSearchReply 
    {
        public string Name { get; set; }
        public double Payout { get; set; }
    }
}
