using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;
using PersonManagment.Infrustructure.Persistence;

namespace TVA.Infrastructure.Repositories
{
    public class SearchAccount : ISearchAccount
    {
        private readonly PersonManagmentDbContext _dbContext;
        public SearchAccount(PersonManagmentDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Account> GetAccountByAccountId(int accountId)
        {
            var account = await _dbContext.Accounts.Include(t=> t.Transactions).AsNoTracking().Where(a => a.AccountId == accountId).FirstOrDefaultAsync();
            return account;
        }

        public async Task<Account> GetAccountByAccountNumber(string accountNumber)
        {
            var account = await _dbContext.Accounts.Where(a => a.AccountNumber == accountNumber).FirstOrDefaultAsync();
            return account;
        }
    }
}
