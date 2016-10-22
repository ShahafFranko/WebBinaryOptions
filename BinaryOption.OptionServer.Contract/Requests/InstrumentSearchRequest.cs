using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.Requests
{
    public class InstrumentSearchRequest
    {
        public InstrumentSearchRequest(string name, double payout, bool disabled)
        {
            Name = name;
            Payout = payout;
            Disabled = disabled;
        }
        public string Name { get; set; }
        public double Payout { get; set; }
        public bool Disabled { get; set; }
    }
}
