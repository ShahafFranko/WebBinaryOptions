using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.Events
{
    public class InstrumentUpdated
    {
        public InstrumentUpdated(string name, double rate)
        {
            Name = name;
            Rate = rate;
        }

        public string Name { get; private set; }
        public double Rate { get; private set; }
    }
}
