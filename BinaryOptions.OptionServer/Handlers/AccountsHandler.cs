using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Autofac;
using BinaryOption.OptionServer.Contract;
using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Entities;
using BinaryOption.OptionServer.Contract.Requests;
using BinaryOptions.DAL;
using BinaryOptions.DAL.Data;

namespace BinaryOptions.OptionServer.Handlers
{
    public class AccountsHandler : ReceiveActor
    {
        private readonly Protocol m_protocol;

        public AccountsHandler(Protocol protocol)
        {
            m_protocol = protocol;
            Receive<CreateAccountRequest>(r => Handle(r));
            Receive<AccountRequest>(r => Handle(r));
        }

        private void Handle(CreateAccountRequest request)
        {
            var account = new Account()
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = request.Password,
                Balance = request.Balance
            };

            try
            {
                using (AccountContext ctx = AccountContext.Create())
                {
                    ctx.Accounts.Add(account);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
               // TODO: Elroie handle errors.
            }

            // Since the account just created, we are creating an empty positions list,
            Sender.Tell(new AccountReply(account.Id, request.Username, request.Balance, new List<PositionDto>()), Self);
        }

        private void Handle(AccountRequest request)
        {
            using (AccountContext ctx = AccountContext.Create())
            {
                Account account = ctx.Accounts.SingleOrDefault(a => a.Id == request.AccountId);

                if (account == null)
                {
                    // TODO: handle errors.

                    return;
                }

                IList<PositionDto> positions = ctx.Positions.Where(p => p.AccountId == account.Id).Select(ConvertToDto).ToList();
                
                AccountReply reply = new AccountReply(account.Id, account.Username, account.Balance, positions);

                Sender.Tell(reply, Self);
            }
        }

        private PositionDto ConvertToDto(Position position)
        {
            return new PositionDto(position.Id, position.AccountId, position.InstrumentName, position.Amount, position.OpenTime, position.ExpireTime,
                position.OpenPrice, position.ClosePrice, (BinaryOption.OptionServer.Contract.Entities.Direction) position.Direction);
        }
    }
}
