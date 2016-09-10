using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryOptions.DAL.Data;

namespace BinaryOptions.DAL
{
    public class DatabaseConfigurations : DbConfiguration
    {
        public DatabaseConfigurations()
        {
            //http://www.entityframeworktutorial.net/entityframework6/code-based-configuration.aspx
            this.SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);
        }
    }
}
