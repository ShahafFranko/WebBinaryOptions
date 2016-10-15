using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BinaryOptions.WebServer.Models
{
    public class PositionsSearchModel
    {
        public DateTime OpenTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public bool Descending { get; set; }
        public bool Wins { get; set; }
    }
}