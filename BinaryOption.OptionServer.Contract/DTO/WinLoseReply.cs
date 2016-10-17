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
            Data.Add(new KeyValuePair<string, double>("Win", Math.Round(winPrecentage * 100, 2)));
            Data.Add(new KeyValuePair<string, double>("Lose", Math.Round(losePrecentage * 100, 2)));
        }

        public IList<KeyValuePair<string, double>> Data { get; private set; }
    }
}
