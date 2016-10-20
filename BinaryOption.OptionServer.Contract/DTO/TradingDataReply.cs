using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.DTO
{
    public class TradingDataReply
    {
        public TradingDataReply(double winPrecentage, double losePrecentage, IList<KeyValuePair<string, int>> highLowPositions)
        {
            HighLowRatio = new List<KeyValuePair<string, int>>();
            WinLoseRatio = new List<KeyValuePair<string, double>>();

            WinLoseRatio.Add(new KeyValuePair<string, double>("Win", Math.Round(winPrecentage * 100, 2)));
            WinLoseRatio.Add(new KeyValuePair<string, double>("Lose", Math.Round(losePrecentage * 100, 2)));

            if (highLowPositions != null)
            {
                foreach (KeyValuePair<string, int> highLowRatio in highLowPositions)
                {
                    HighLowRatio.Add(highLowRatio);
                }    
            }
        }

        public IList<KeyValuePair<string, int>> HighLowRatio { get; private set; }
        public IList<KeyValuePair<string, double>> WinLoseRatio { get; private set; }
    }
}
