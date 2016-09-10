using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryOption.OptionServer.Contract.Entities;

namespace BinaryOption.OptionServer.Contract.Commands
{
    public class ClosePositionCommand
    {
        public ClosePositionCommand(Guid accountId, Guid positionId, string instrumentName)
        {
            AccountId = accountId;
            PositionId = positionId;
            InstrumentName = instrumentName;
        }

        public Guid AccountId { get; private set; }
        public Guid PositionId { get; private set; }
        public string InstrumentName { get; private set; }
    }
}
