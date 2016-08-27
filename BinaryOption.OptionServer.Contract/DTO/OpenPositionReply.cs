using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.DTO
{
    public class OpenPositionReply : IReply
    {
        public OpenPositionReply(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }

        public bool IsSuccessful { get; private set; }
    }
}
