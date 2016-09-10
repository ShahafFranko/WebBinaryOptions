using BinaryOption.OptionServer.Contract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.Requests
{
    public class OpenPositionRequest
    {
        public OpenPositionRequest(Guid accountId, Direction direction, string instrumentName, int amount)
        {
            AccountId = accountId;
            Direction = direction;
            InstrumentName = instrumentName;
            Amount = amount;
        }

        public Guid AccountId { get; private set; }
        public Direction Direction { get; private set; }
        public string InstrumentName { get; private set; }
        public int Amount { get; private set; }
    }
}
