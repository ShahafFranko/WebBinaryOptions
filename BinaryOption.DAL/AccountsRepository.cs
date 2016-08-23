using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryOption.DAL.Data;

namespace BinaryOption.DAL
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
    }
}
