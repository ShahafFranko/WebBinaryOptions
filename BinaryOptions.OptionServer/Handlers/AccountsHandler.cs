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
            Receive<GetAccountRequest>(r => Handle(r));
            Receive<DeleteAccountRequest>(r => Handle(r));
            Receive<AccountDepositRequest>(r => Handle(r));
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
                using (BinaryOptionsContext ctx = BinaryOptionsContext.Create())
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
            using (BinaryOptionsContext ctx = BinaryOptionsContext.Create())
            {
                Account account = ctx.Accounts.SingleOrDefault(a => a.Id == request.AccountId);

                if (account == null)
                {
                    // TODO: handle errors.

                    return;
                }

                Sender.Tell(FromAccount(account), Self);
            }
        }

        private void Handle(GetAccountRequest request)
        {
            using (BinaryOptionsContext ctx = BinaryOptionsContext.Create())
            {
                IEnumerable<Account> accounts = ctx.Accounts.ToList();

                IList<AccountReply> accountsReply = accounts.Select(a => FromAccount(a)).ToList();

                Sender.Tell(accountsReply, Self);
            }
        }

        private void Handle(DeleteAccountRequest request)
        {
            using (var ctx = BinaryOptionsContext.Create())
            {
                Account account = ctx.Accounts.Find(request.AccountId);

                if (account == null)
                {
                    Sender.Tell(false, Self);
                    return;
                }

                ctx.Accounts.Remove(account);
                ctx.SaveChanges();
                Sender.Tell(true, Self);
            }
        }

        private void Handle(AccountDepositRequest request)
        {
            using (var ctx = BinaryOptionsContext.Create())
            {
                Account account = ctx.Accounts.SingleOrDefault(a => a.Username == request.Username);

                if (account == null)
                {
                    Sender.Tell(false, Self);
                    return;
                }

                account.Balance += request.Amount;
                ctx.SaveChanges();
                Sender.Tell(true, Self);
            }
        }

        private static PositionDto FromPosition(Position position)
        {
            return new PositionDto(position.Id, position.AccountId, position.InstrumentName, position.Amount,
                position.OpenTime, position.ExpireTime, position.OpenPrice, position.ClosePrice,
                (BinaryOption.OptionServer.Contract.Entities.Direction)position.Direction);
        }

        private static AccountReply FromAccount(Account account)
        {
            IList<PositionDto> positions = account.Positions.Select(FromPosition).OrderByDescending(p => p.ExpireTime).ToList();
            return new AccountReply(account.Id, account.Username, account.Balance, positions);
        }

    }
}
