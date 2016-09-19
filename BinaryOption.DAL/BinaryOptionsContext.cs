using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BinaryOption.DAL;
using BinaryOptions.DAL.Data;

namespace BinaryOptions.DAL
{
    public class BinaryOptionsContext : DbContext
    {
        private BinaryOptionsContext()
            : base(@"Server=localhost;Data Source=.;Initial Catalog=BinaryOptions;Integrated Security=true")
        {
            Database.SetInitializer(new DataInitializer());
            Database.Initialize(false);
        }

        public DbSet<Position> Positions { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public static BinaryOptionsContext Create()
        {
            return new BinaryOptionsContext();
        }
    }
}
