using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.DTO
{
    public class WinLoseReply
    {
        public WinLoseReply(double winPrecentage, double losePrecentage)
        {
            Data = new List<KeyValuePair<string, double>>();
            Data.Add(new KeyValuePair<string, double>("Win", winPrecentage * 100));
            Data.Add(new KeyValuePair<string, double>("Lose", losePrecentage * 100));
            //WinPrecentage = winPrecentage*100;
            //LosePrecentage = losePrecentage * 100;
        }

        public IList<KeyValuePair<string, double>> Data { get; private set; }
        //public double WinPrecentage { get; private set; }
        //public double LosePrecentage { get; private set; }
    }
}
