using BinaryOption.OptionServer.Contract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BinaryOptions.WebServer.Models
{
    public class OpenPositionModel
    {
        public string Username { get; set; }
        public Direction Direction { get; set; }
        public string InstrumentName { get; set; }
        public int Amount { get; set; }
    }
}