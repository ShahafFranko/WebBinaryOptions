using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryOption.DAL.Data;

namespace BinaryOption.DAL
{
    public class DBContext : DbContext
    {
        public DBContext() : base("BinaryOptions")
        {
        }

        public DbSet<Position> Positions { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}
