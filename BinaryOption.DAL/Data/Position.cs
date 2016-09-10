using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOptions.DAL.Data
{
    // Represents Positions Table.
    public class Position
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string InstrumentName { get; set; }
        public int Amount { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public double OpenPrice { get; set; }
        public double? ClosePrice { get; set; }
        public Direction Direction { get; set; }
    }
}
