using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryOptions.DAL;
using BinaryOptions.DAL.Data;

namespace BinaryOption.DAL
{
    public class DataInitializer : CreateDatabaseIfNotExists<BinaryOptionsContext>
    {
        protected override void Seed(BinaryOptionsContext context)
        {
            Account adminAccount = new Account() { Id = Guid.NewGuid(), Username = "admin", Password = "ab1234", Balance = 0 };
            context.Accounts.Add(adminAccount);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
