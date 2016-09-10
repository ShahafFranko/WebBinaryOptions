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
    public class OpenPositionCommandHandler : ReceiveActor
    {
        private readonly ActorSystem m_actorSystem;
        private readonly InstrumentRepository m_instrumentRepository;
        private readonly Protocol m_protocol;

        public OpenPositionCommandHandler(ActorSystem actorSystem, InstrumentRepository instrumentRepository, Protocol protocol)
        {
            m_actorSystem = actorSystem;
            m_instrumentRepository = instrumentRepository;
            m_protocol = protocol;
            Receive<OpenPositionRequest>(c => Handle(c));
        }

        private void Handle(OpenPositionRequest request)
        {
            Position openPosition = null;
            Account account = null;
            Instrument instrument = m_instrumentRepository.GetInstrument(request.InstrumentName);            

            try
            {
                using (var ctx = AccountContext.Create())
                {
                    account = ctx.Accounts.SingleOrDefault(a => a.Id == request.AccountId);

                    if (!Validate(account, instrument, request))
                    {
                        // Handle errors.
                        return;
                    }

                    openPosition = new Position()
                    {
                        Id = Guid.NewGuid(),
                        AccountId = account.Id,
                        InstrumentName = request.InstrumentName,
                        Amount = request.Amount,
                        OpenPrice = instrument.Rate,
                        OpenTime = DateTime.Now,
                        ExpireTime = DateTime.Now.AddMinutes(1),
                        ClosePrice = null,
                        Direction = (Direction)request.Direction
                    };

                    ctx.Positions.Add(openPosition);

                    // reduce the position amount from account balance.
                    account.Balance -= request.Amount;

                    ctx.SaveChanges();
                }
            }
            catch (Exception)
            {
                // Handle errors.
            }

            // Generate path for an inproces actor.
            ActorSelection actorSelection = m_actorSystem.ActorSelection(m_protocol.GenerateInprocPath("ClosePositionCommandHandler"));
            var closePositionsHandler = actorSelection.ResolveOne(TimeSpan.FromSeconds(1)).Result;

            // schedule a close command according to the openPosition expire time.
            TimeSpan expireOn = openPosition.ExpireTime - openPosition.OpenTime;
            m_actorSystem.Scheduler.ScheduleTellOnce(expireOn, closePositionsHandler, new ClosePositionCommand(account.Id, openPosition.Id, instrument.Name), closePositionsHandler);

            Sender.Tell(new OpenPositionReply(true));
        }

        private bool Validate(Account account, Instrument instrument, OpenPositionRequest request)
        {
            // if account doesn't exist.
            if (account == null)
            {
                // Handle erros.
                return false;
            }

            // if account doesn't have sufficient funds to open a position.
            if (account.Balance < request.Amount)
            {
                return false;
            }

            if (instrument == null)
            {
                return false;
            }

            return true;
        }
    }
}
