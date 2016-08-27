using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryOption.OptionServer.Contract.Entities;

namespace BinaryOption.OptionServer.Contract.Commands
{
    public class OpenPositionCommand
    {
        public Guid AccountId { get; set; }
        public Guid InstrumentId { get; set; }
        public int Amount { get; set; }
        public DateTime OpenTime { get; set; }
        public double OpenPrice { get; set; }
        public Direction Direction { get; set; }
    }
}
