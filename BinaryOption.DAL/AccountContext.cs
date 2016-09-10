using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BinaryOptions.DAL.Data;

namespace BinaryOptions.DAL
{
    public class AccountContext : DbContext
    {
        private AccountContext()
            : base(@"Server=localhost;Data Source=.;Initial Catalog=BinaryOptions;Integrated Security=true")
        {
        }

        public DbSet<Position> Positions { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public static AccountContext Create()
        {
            return new AccountContext();
        }
    }
}
