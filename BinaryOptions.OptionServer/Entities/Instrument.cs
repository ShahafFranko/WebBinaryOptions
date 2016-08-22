using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOptions.OptionServer.Entities
{
    public class Instrument
    {
        public Instrument(string name, double min, double max)
        {
            Name = name;
        }
        public string Name { get; private set; }
        public double Min { get; private set; }
        public double Max { get; private set; }
        public double Rate { get; private set; }

        public void Update(double rate) 
        {
            Rate = rate;
        }
    }
}
