using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract
{
    public class Protocol
    {
        private readonly string TcpTemplate;
        private readonly string InProcTemplate;

        public Protocol(string actorSystem, string remoteSystem, int remotePort)
        {
            InProcTemplate = string.Format("akka://{0}/user/", actorSystem);
            TcpTemplate = string.Format("akka.tcp://{0}@localhost:{1}/user/", remoteSystem, remotePort);
        }

        public string GenerateTcpPath(string actorName)
        {
            return TcpTemplate + actorName;
        }

        public string GenerateInprocPath(string actorName)
        {
            return InProcTemplate + actorName;
        }
    }
}
