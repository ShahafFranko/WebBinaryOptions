using BinaryOption.OptionServer.Contract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.Requests
{
    public class InstrumentRatesRequest
    {
        public InstrumentRatesRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
