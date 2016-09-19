using System;

namespace BinaryOptions.DAL.Data
{
    public class InstrumentRate
    {
        public InstrumentRate(Guid id, double rate, DateTime time)
        {
            Id = id;
            Rate = rate;
            Time = time;
        }

        public Guid Id { get; private set; }
        public double Rate { get; private set; }
        public DateTime Time { get; private set; }
    }
}
