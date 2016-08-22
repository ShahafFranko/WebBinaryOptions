using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOptions.OptionServer.Entities
{
    public class Position
    {
        public Position(Instrument instrument, int amount)
        {

        }

        public Guid Id { get; set; }
        public Guid InstrumentId { get; set; }
        public int Amount { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public double OpenPrice { get; set; }
        public double? ClosePrice { get; set; }

        public bool IsOpen() 
        {
            return ClosePrice.HasValue;
        }
    }
}
