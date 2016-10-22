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
            Receive<InstrumentSearchRequest>(c => Handle(c));
            Receive<TradingDataRequest>(c => Handle(c));
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
                        positions = positions.OrderByDescending(p => p.OpenTime);
                    }
                    else
                    {
                        positions = positions.OrderBy(p => p.OpenTime);
                    }

                    IEnumerable<PositionDto> results = positions.ToList().Select(FromPosition);

                    Sender.Tell(results.ToList(), Self);
                }
            }
            catch (Exception ex)
            {
                Context.GetLogger().Error("Search Failed", ex);
            }
        }

        private void Handle(TradingDataRequest request)
        {
            try
            {
                using (var ctx = BinaryOptionsContext.Create())
                {
                    TradingDataReply reply = null;

                    IQueryable<Position> positions = ctx.Positions;

                    // group by direction and count each positions per direction.
                    IList<KeyValuePair<string, int>> highLowPositions = 
                        ctx.Positions.GroupBy(p => p.Direction.ToString())
                        .ToDictionary(item => item.Key, item => item.Count())
                        .ToList();

                    int totalPositions = positions.Count();
                    if (totalPositions == 0)
                    {
                        reply = new TradingDataReply(0, 0, highLowPositions);

                        Sender.Tell(reply, Self);
                        return;
                    }

                    double winningPositions = positions.Count(p => (p.Direction == Direction.High && p.ClosePrice > p.OpenPrice) ||
                                                        (p.Direction == Direction.Low && p.OpenPrice > p.ClosePrice));

                    double losingPositions = totalPositions - winningPositions;



                    reply = new TradingDataReply(winningPositions / totalPositions, losingPositions / totalPositions, highLowPositions);
                    
                    Sender.Tell(reply, Self);
                }
            }
            catch (Exception ex)
            {
                Context.GetLogger().Error("Search Failed", ex);
            }
        }

        private void Handle(InstrumentSearchRequest request) 
        {
            using (var ctx = BinaryOptionsContext.Create())
            {
                IEnumerable<Instrument> instruments = Enumerable.Empty<Instrument>();

                if (string.IsNullOrEmpty(request.Name))
                {
                    instruments = ctx.Instruments.Where(i => i.Payout >= request.Payout && i.IsEnabled == request.Disabled);
                }
                else
                {
                    instruments = ctx.Instruments.Where(i => i.Name.ToLower().Contains(request.Name.ToLower())
                        && i.Payout >= request.Payout && i.IsEnabled == request.Disabled);
                }

                IList<InstrumentSearchReply> searchData 
                    = instruments.Select(i => new InstrumentSearchReply { Name = i.Name, Payout = i.Payout }).ToList();
                
                Sender.Tell(searchData, Self);
            }
        }

        private static PositionDto FromPosition(Position position)
        {
            return new PositionDto(position.Id, position.AccountId, position.InstrumentName, position.Amount,
                position.OpenTime, position.ExpireTime, position.OpenPrice, position.ClosePrice,
                (BinaryOption.OptionServer.Contract.Entities.Direction)position.Direction);
        }
    }
}
