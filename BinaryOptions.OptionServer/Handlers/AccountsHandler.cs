using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Autofac;
using BinaryOption.OptionServer.Contract;
using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Requests;

namespace BinaryOptions.OptionServer.Handlers
{
    public class AccountsHandler : ReceiveActor
    {
        private readonly Protocol m_protocol;

        public AccountsHandler(Protocol protocol)
        {
            m_protocol = protocol;
            Receive<CreateAccountRequest>(r => Handle(r));
        }

        private void Handle(CreateAccountRequest request)
        {
            // validate account's details.

            // if valid save to db.

            // return Creation Reply

            Sender.Tell(new AccountReply(request.Username, request.Password, request.Balance), Self);
        }
    }
}
