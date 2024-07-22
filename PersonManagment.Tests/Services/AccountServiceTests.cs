using Moq;
using PersonManagment.Application.Services;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonManagment.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IRepository<Account>> _accountRepositoryMock;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _accountRepositoryMock = new Mock<IRepository<Account>>();
            _accountService = new AccountService(_accountRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAccountsAsync_ReturnsAllAccounts()
        {
            // Arrange
            var accounts = new List<Account> { new Account(), new Account() };
            _accountRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(accounts);

            // Act
            var result = await _accountService.GetAccountsAsync();

            // Assert
            Assert.Equal(accounts.Count, result.Count());
            _accountRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAccountByIdAsync_ReturnsAccount()
        {
            // Arrange
            var account = new Account { AccountId = 1 };
            _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(account);

            // Act
            var result = await _accountService.GetAccountByIdAsync(1);

            // Assert
            Assert.Equal(account.AccountId, result.AccountId);
            _accountRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task CreateAccountAsync_AddsAccount()
        {
            // Arrange
            var account = new Account { AccountId = 1 };
            _accountRepositoryMock.Setup(repo => repo.AddAsync(account)).ReturnsAsync(account);

            // Act
            var result = await _accountService.CreateAccountAsync(account);

            // Assert
            Assert.Equal(account.AccountId, result.AccountId);
            _accountRepositoryMock.Verify(repo => repo.AddAsync(account), Times.Once);
        }

        [Fact]
        public async Task UpdateAccountAsync_UpdatesAccount()
        {
            // Arrange
            var account = new Account { AccountId = 1 };

            // Act
            await _accountService.UpdateAccountAsync(account);

            // Assert
            _accountRepositoryMock.Verify(repo => repo.UpdateAsync(account), Times.Once);
        }

        [Fact]
        public async Task DeleteAccountAsync_DeletesAccount()
        {
            // Arrange
            var accountId = 1;

            // Act
            await _accountService.DeleteAccountAsync(accountId);

            // Assert
            _accountRepositoryMock.Verify(repo => repo.DeleteAsync(accountId), Times.Once);
        }

        [Fact]
        public async Task CloseAccount_WithZeroBalance_ClosesAccount()
        {
            // Arrange
            var account = new Account { AccountId = 1, Balance = 0.00m, IsClosed = false };
            _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(account);

            // Act
            var result = await _accountService.CloseAccount(1);

            // Assert
            Assert.True(result);
            Assert.True(account.IsClosed);
            _accountRepositoryMock.Verify(repo => repo.UpdateAsync(account), Times.Once);
        }

        [Fact]
        public async Task CloseAccount_WithNonZeroBalance_DoesNotCloseAccount()
        {
            // Arrange
            var account = new Account { AccountId = 1, Balance = 100.00m, IsClosed = false };
            _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(account);

            // Act
            var result = await _accountService.CloseAccount(1);

            // Assert
            Assert.False(result);            
        }      

        [Fact]
        public async Task UpdateAccountBalanceOnNewTransaction_UpdatesBalance()
        {
            // Arrange
            var account = new Account { AccountId = 1, Balance = 100.00m };
            var transactionAmount = 50.00m;
            _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(account);

            // Act
            var result = await _accountService.UpdateAccountBalanceOnNewTransaction(1, transactionAmount);

            // Assert
            Assert.True(result);
            Assert.Equal(150.00m, account.Balance);
            _accountRepositoryMock.Verify(repo => repo.UpdateAsync(account), Times.Once);
        }

        [Fact]
        public async Task UpdateAccountBalanceOnExistingTransaction_UpdatesBalance()
        {
            // Arrange
            var account = new Account { AccountId = 1, Balance = 100.00m };
            decimal existingAmount = 200.00m;
            decimal newAmount = 150.00m;
            _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(account);

            // Act
            var result = await _accountService.UpdateAccountBalanceOnExistingTransaction(account.AccountId,existingAmount,newAmount);

            // Assert
            Assert.True(result);
            //Assert.Equal(250.00m, account.Balance);
            _accountRepositoryMock.Verify(repo => repo.UpdateAsync(account), Times.Once);
        }
    }
}
