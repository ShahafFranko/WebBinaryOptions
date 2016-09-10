using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;
using BinaryOption.DAL.Repositories;
using BinaryOption.OptionServer.Contract;
using BinaryOption.OptionServer.Contract.Commands;
using BinaryOptions.DAL;
using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Requests;
using BinaryOptions.DAL.Data;

namespace BinaryOptions.OptionServer.Handlers
{
    public class ClosePositionCommandHandler : ReceiveActor
    {
        private readonly InstrumentRepository m_instrumentRepository;
        private readonly Protocol m_protocol;

        public ClosePositionCommandHandler(InstrumentRepository instrumentRepository, Protocol protocol)
        {
            m_instrumentRepository = instrumentRepository;
            m_protocol = protocol;
            Receive<ClosePositionCommand>(c => Handle(c));
        }

        private void Handle(ClosePositionCommand command)
        {
            try
            {
                using (var ctx = AccountContext.Create())
                {
                    Account account = ctx.Accounts.SingleOrDefault(c => c.Id == command.AccountId);
                    Instrument instrument = m_instrumentRepository.GetInstrument(command.InstrumentName);
                    Position position = ctx.Positions.SingleOrDefault(p => p.Id == command.PositionId);

                    if (!Validate(account, instrument, position))
                    {
                        // Handle Errors.
                        return;
                    }

                    double currentRate = instrument.Rate;

                    // two situation account can win, if he open High position and the the instruemnt rate goes higher.
                    // or he opens Low position and the instrument rate goes lower. 
                    if ((position.Direction == Direction.High && position.OpenPrice < currentRate) ||
                        (position.Direction == Direction.Low && position.OpenPrice > currentRate))
                    {
                        // We returning account investment + the option payout.
                        account.Balance += position.Amount + (position.Amount * instrument.Payout);
                    }

                    // update the position close price.
                    position.ClosePrice = currentRate;

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Handle errors.
                return;
            }
        }

        private bool Validate(Account account, Instrument instrument, Position position)
        {
            // if account doesn't exist.
            if (account == null)
            {
                // Handle erros.
                return false;
            }

            if (instrument == null)
            {
                return false;
            }

            if (position == null)
            {
                return false;
            }

            return true;
        }
    }
}
