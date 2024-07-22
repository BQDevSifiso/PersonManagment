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
    public class TransactionServiceTests
    {
        private readonly Mock<IRepository<Transaction>> _transactionRepositoryMock;
        private readonly TransactionService _transactionService;

        public TransactionServiceTests()
        {
            _transactionRepositoryMock = new Mock<IRepository<Transaction>>();
            _transactionService = new TransactionService(_transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task GetTransactionsAsync_ReturnsAllTransactions()
        {
            // Arrange
            var transactions = new List<Transaction> { new Transaction(), new Transaction() };
            _transactionRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(transactions);

            // Act
            var result = await _transactionService.GetTransactionsAsync();

            // Assert
            Assert.Equal(transactions.Count, result.Count());
            _transactionRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTransactionByIdAsync_ReturnsTransaction()
        {
            // Arrange
            var transaction = new Transaction { TransactionId = 1 };
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(transaction);

            // Act
            var result = await _transactionService.GetTransactionByIdAsync(1);

            // Assert
            Assert.Equal(transaction.TransactionId, result.TransactionId);
            _transactionRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task CreateTransactionAsync_AddsTransaction()
        {
            // Arrange
            var transaction = new Transaction { TransactionId = 1 };
            _transactionRepositoryMock.Setup(repo => repo.AddAsync(transaction)).ReturnsAsync(transaction);

            // Act
            var result = await _transactionService.CreateTransactionAsync(transaction);

            // Assert
            Assert.Equal(transaction.TransactionId, result.TransactionId);
            _transactionRepositoryMock.Verify(repo => repo.AddAsync(transaction), Times.Once);
        }

        [Fact]
        public async Task UpdateTransactionAsync_UpdatesTransaction()
        {
            // Arrange
            var transaction = new Transaction { TransactionId = 1 };

            // Act
            await _transactionService.UpdateTransactionAsync(transaction);

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.UpdateAsync(transaction), Times.Once);
        }

        [Fact]
        public async Task DeleteTransactionAsync_DeletesTransaction()
        {
            // Arrange
            var transactionId = 1;

            // Act
            await _transactionService.DeleteTransactionAsync(transactionId);

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.DeleteAsync(transactionId), Times.Once);
        }
    }
}
