using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryOptions.DAL.Data;

namespace BinaryOptions.DAL
{
    public class AccountsRepository
    {
        private readonly DBContext m_context;

        public AccountsRepository(DBContext context)
        {
            m_context = context;
        }

        public Account GetAccountById(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> GetAccounts()
        {
            throw new NotImplementedException();
        }

        public Account TryAuthenticate(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
