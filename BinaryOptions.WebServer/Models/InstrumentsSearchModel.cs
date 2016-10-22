using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BinaryOptions.WebServer.Models
{
    public class InstrumentsSearchModel
    {
        public string Name { get; set; }
        public double Payout { get; set; }
        public bool Disabled { get; set; }
    }
}