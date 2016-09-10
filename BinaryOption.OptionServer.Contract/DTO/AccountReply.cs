using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.DTO
{
    public class AccountReply : IReply
    {
        public AccountReply(Guid id, string username, double balance, IList<PositionDto> positions)
        {
            Id = id;
            Username = username;
            Balance = Math.Round(balance, 2);
            Positions = positions;
        }

        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public double Balance { get; private set; }
        public IList<PositionDto> Positions { get; set; }
    }
}
