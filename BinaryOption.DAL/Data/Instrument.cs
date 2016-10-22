using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BinaryOptions.DAL.Data
{
    public class Instrument
    {
        public Instrument()
        {

        }

        public Instrument(string name, double min, double max, double payout, bool isEnabled = true)
        {
            Id = Guid.NewGuid();
            Name = name;
            Min = min;
            Max = max;
            Payout = payout;
            IsEnabled = isEnabled;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Rate { get; set; }
        public double Payout { get; set; }
        public bool IsEnabled { get; set; }

        public void Update(double rate) 
        {
            Rate = rate;
        }
    }
}
