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

        public Account(string username, string password)
        {
            m_openPositions = new List<Position>();
            m_closePositions = new List<Position>();
        }

        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Balance { get; private set; }

        public void OpenPosition(Instrument instrument, int amount) 
        {
            // create new position.
            // add position to m_openPositions;
        }

        public void ClosePosition(Guid positionId, double closeRate)
        {
            // locate position.
            // call position.Close();
        }
    }
}
