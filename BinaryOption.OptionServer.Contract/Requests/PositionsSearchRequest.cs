using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.Requests
{
    public class PositionsSearchRequest
    {
        public PositionsSearchRequest(DateTime openTime, DateTime expireTime, bool sortDescending, bool sortWinsOnly)
        {
            OpenTime = openTime;
            ExpireTime = expireTime;
            SortDescending = sortDescending;
            SortWinsOnly = sortWinsOnly;
        }

        public DateTime OpenTime { get; private set; }
        public DateTime ExpireTime { get; private set; }
        public bool SortDescending { get; private set; }
        public bool SortWinsOnly { get; private set; }
    }
}
