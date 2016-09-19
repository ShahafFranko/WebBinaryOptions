using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using BinaryOption.OptionServer.Contract;
using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Requests;
using BinaryOptions.DAL;
using BinaryOptions.DAL.Data;

namespace BinaryOptions.OptionServer.Handlers
{
    public class LoginRequestHandler : ReceiveActor
    {
        private readonly Protocol m_protocol;

        public LoginRequestHandler(Protocol protocol)
        {
            m_protocol = protocol;
            Receive<LoginRequest>(r => Handle(r));
        }

        public void Handle(LoginRequest request)
        {
            try
            {
                using (var ctx = BinaryOptionsContext.Create())
                {
                    Account account = ctx.Accounts.SingleOrDefault(a => a.Username == request.Username && a.Password == request.Password);

                    if (account != null)
                    {
                        Sender.Tell(new LoginReply(true, account.Id));
                    }
                    else
                    {
                        Sender.Tell(new LoginReply(false, Guid.Empty));
                    }

                }
            }
            catch (Exception ex)
            {
                // Handle errors.
                Sender.Tell(new LoginReply(false, Guid.Empty));
            }
        }
    }
}
