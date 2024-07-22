using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonManagment.Domain.Entities;

namespace PersonManagment.Domain.Interfaces
{
    public interface ISearchAccount
    {
        Task<Account> GetAccountByAccountNumber(string accountNumber);
        Task<Account> GetAccountByAccountId(int accountId);
    }
}
