using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;

namespace PersonManagment.Application.Services
{
    public class AccountService
    {
        private readonly IRepository<Account> _accountRepository;
        public AccountService(IRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        public async Task<Account> GetAccountByIdAsync(int accountId)
        {
            return await _accountRepository.GetByIdAsync(accountId);
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            account.AccountNumber = GenerateSecureSixDigitNumber().ToString();
            account.CreatedDate = DateTime.UtcNow;
            return await _accountRepository.AddAsync(account);
        }

        public async Task UpdateAccountAsync(Account account)
        {
            await _accountRepository.UpdateAsync(account);
        }

        public async Task DeleteAccountAsync(int accountId)
        {
            await _accountRepository.DeleteAsync(accountId);
        }

        public async Task<bool> CloseAccount(int accountId)
        {
            bool isClosed = false;

            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account != null)
            {
                if (account.Balance == 0.00m)
                {
                    isClosed = true;
                    account.IsClosed = true;
                    account.ClosedDate = DateTime.UtcNow;
                    await _accountRepository.UpdateAsync(account);
                }
                else
                {
                    isClosed = false;
                }
            }
            return isClosed;
        }

        public async Task<bool> UpdateAccountBalanceOnNewTransaction(int accountId, decimal TransactionAmount)
        {
            bool isUpdate = false;
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account != null)
            {
                account.Balance += TransactionAmount;
                await _accountRepository.UpdateAsync(account);
                isUpdate = true;
            }
            return isUpdate;
        }
        public async Task<bool> UpdateAccountBalanceOnExistingTransaction(int accountId, decimal existingAmount, decimal newAmount)
        {
            bool isUpdate = false;
            decimal amountDiff = 0.00m;

            var account = await _accountRepository.GetByIdAsync(accountId);

            if (account != null)
            {
                if (newAmount < existingAmount)
                {
                    amountDiff = Math.Abs(existingAmount - newAmount);
                    account.Balance = account.Balance + amountDiff;
                }
                else
                {
                    amountDiff = Math.Abs(newAmount - existingAmount);
                    account.Balance = account.Balance - amountDiff;
                }
                await _accountRepository.UpdateAsync(account);
                isUpdate = true;
            }
            return isUpdate;
        }

        private int GenerateSecureSixDigitNumber()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                int randomValue = BitConverter.ToInt32(bytes, 0) & int.MaxValue;
                return 100000 + (randomValue % 900000);
            }
        }
    }
}
