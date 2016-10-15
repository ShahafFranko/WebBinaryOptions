using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Event;
using BinaryOption.DAL.Repositories;
using BinaryOption.OptionServer.Contract;
using BinaryOption.OptionServer.Contract.Commands;
using BinaryOptions.DAL;
using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Requests;
using BinaryOptions.DAL.Data;

namespace BinaryOptions.OptionServer.Handlers
{
    public class SearchRequestsHandler : ReceiveActor
    {
        public SearchRequestsHandler()
        {
            Receive<PositionsSearchRequest>(c => Handle(c));
        }

        private void Handle(PositionsSearchRequest request)
        {
            try
            {
                using (var ctx = BinaryOptionsContext.Create())
                {
                    
                    IQueryable<Position> positions = ctx.Positions.Where(p => p.OpenTime >= request.OpenTime &&
                        p.ExpireTime <= request.ExpireTime);

                    if (request.SortWinsOnly)
                    {
                        positions = positions.Where(p => (p.Direction == Direction.High && p.ClosePrice > p.OpenPrice) ||
                                                        (p.Direction == Direction.Low && p.OpenPrice > p.ClosePrice));
                    }

                    if (request.SortDescending)
                    {
                        positions = positions.OrderByDescending(p => p.Amount);
                    }

                    IEnumerable<PositionDto> results = positions.OrderBy(p => p.Amount).ToList().Select(FromPosition);

                    Sender.Tell(results.ToList(), Self);
                }
            }
            catch (Exception ex)
            {
                Context.GetLogger().Error("Search Failed", ex);
            }
        }

        private static PositionDto FromPosition(Position position)
        {
            return new PositionDto(position.Id, position.AccountId, position.InstrumentName, position.Amount,
                position.OpenTime, position.ExpireTime, position.OpenPrice, position.ClosePrice,
                (BinaryOption.OptionServer.Contract.Entities.Direction)position.Direction);
        }

        public bool IsWinning(Position position)
        {
            if (!position.ClosePrice.HasValue)
            {
                return false;
            }

            if ((position.Direction == Direction.High && position.ClosePrice > position.OpenPrice) ||
                (position.Direction == Direction.Low && position.OpenPrice > position.ClosePrice))
            {
                return true;
            }

            return false;
        }
    }
}
