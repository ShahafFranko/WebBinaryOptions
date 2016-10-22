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
            context.Instruments.Add(new Instrument("EURUSD", 1.0, 2.0, 0.6));
            context.Instruments.Add(new Instrument("EURGBP", 1.1, 2.5, 0.67));
            context.Instruments.Add(new Instrument("GBPUSD", 1.2, 2.4, 0.66));
            context.Instruments.Add(new Instrument("USDJPY", 14.1, 23.23, 0.62));
            context.Instruments.Add(new Instrument("EURJPY", 12.1, 20.23, 0.9, false));
           
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
