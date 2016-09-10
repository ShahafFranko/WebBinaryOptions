using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryOption.OptionServer.Contract.Entities;

namespace BinaryOption.OptionServer.Contract.DTO
{
    public class PositionDto
    {
        public PositionDto(Guid id, Guid accountId, string instrumentName, int amount, DateTime openTime,
            DateTime expireTime, double openPrice, double? closePrice, Direction direction)
        {
            Id = id;
            AccountId = accountId;
            InstrumentName = instrumentName;
            Amount = amount;
            OpenTime = openTime;
            ExpireTime = expireTime;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
            Direction = direction.ToString();
        }

        public Guid Id { get; private set; }
        public Guid AccountId { get; private set; }
        public string InstrumentName { get; private set; }
        public int Amount { get; private set; }
        public DateTime OpenTime { get; private set; }
        public DateTime ExpireTime { get; private set; }
        public double OpenPrice { get; private set; }
        public double? ClosePrice { get; private set; }
        public string Direction { get; private set; }
    }
}
