using Microsoft.EntityFrameworkCore;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;
using PersonManagment.Infrustructure.Persistence;

namespace TVA.Infrastructure.Repositories
{
    public class SearchTransaction : ISearchTransaction
    {
        private readonly PersonManagmentDbContext _dbContext;
        public SearchTransaction(PersonManagmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Transaction>> GetTransactionsByAccoutId(int accountId)
        {
            var transaction = await _dbContext.Transactions.AsNoTracking().Where(t=> t.AccountId == accountId).ToListAsync();
            return transaction;
        }

        public async Task<Transaction> GetTransactionById(int transactionId)
        {
            var transaction = await _dbContext.Transactions.AsNoTracking().Where(t=> t.TransactionId == transactionId).FirstOrDefaultAsync();
            return transaction;
        }
    }
}
