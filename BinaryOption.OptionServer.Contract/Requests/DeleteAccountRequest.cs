using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.Requests
{
    public class DeleteAccountRequest : IRequest
    {
        public DeleteAccountRequest(Guid accountId)
        {
            AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
    }
}
