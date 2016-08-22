using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOptions.OptionServer.Entities
{
    public class Account
    {
        private IList<Position> m_openPositions;
        private IList<Position> m_closePositions;

        public Account()
        {
            m_openPositions = new List<Position>();
            m_closePositions = new List<Position>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Balance { get; set; }

        public void OpenPosition(Instrument instrument, int amount) 
        {
            
        }
    }
}
