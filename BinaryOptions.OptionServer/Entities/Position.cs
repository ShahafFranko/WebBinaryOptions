using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOptions.OptionServer.Entities
{
    public class Position
    {
        public Position()
        {

        }

        public Guid Id { get; private set; }
        public Guid InstrumentId { get; private set; }
        public int Amount { get; private set; }
        public DateTime OpenTime { get; private set; }
        public DateTime ExpireTime { get; private set; }
        public double OpenPrice { get; private set; }
        public double? ClosePrice { get; private set; }
        public Direction Direction { get; private set; }

        public bool IsOpen() 
        {
            return ClosePrice.HasValue;
        }

        public void Close(double closePrice, DateTime expireTime)
        {
            ClosePrice = closePrice;
            ExpireTime = expireTime;
        }
    }
}
