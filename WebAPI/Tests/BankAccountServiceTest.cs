using Moq;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests
{
    public class BankAccountServiceTests
    {
        private readonly Mock<IBankAccountRepository> _repositoryMock;
        private readonly IBankAccountService _service;

        public BankAccountServiceTests()
        {
            _repositoryMock = new Mock<IBankAccountRepository>();
            _service = new BankAccountService(_repositoryMock.Object);
        }

        [Fact]
        public async Task DepositAsync_ShouldIncreaseBalance()
        {
            // Arrange
            var account = new BankAccount { Id = 1, Balance = 100 };
            _repositoryMock.Setup(r => r.GetBankAccountAsync(1)).ReturnsAsync(account);

            // Act
            var result = await _service.DepositAsync(1, 50);

            // Assert
            Assert.Equal(150, result.Balance);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldDecreaseBalance()
        {
            // Arrange
            var account = new BankAccount { Id = 1, Balance = 100 };
            _repositoryMock.Setup(r => r.GetBankAccountAsync(1)).ReturnsAsync(account);

            // Act
            var result = await _service.WithdrawAsync(1, 50);

            // Assert
            Assert.Equal(50, result.Balance);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldThrowException_WhenInsufficientFunds()
        {
            // Arrange
            var account = new BankAccount { Id = 1, Balance = 100 };
            _repositoryMock.Setup(r => r.GetBankAccountAsync(1)).ReturnsAsync(account);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.WithdrawAsync(1, 150));
        }

        [Fact]
        public async Task GetStatementAsync_ShouldReturnAccountStatement()
        {
            // Arrange
            var account = new BankAccount
            {
                Id = 1,
                AccountNumber = "1111",
                Balance = 100,
                Transactions = new List<Transaction>
                {
                    new Transaction { Id = 1, Date = DateTime.UtcNow, Amount = 50, Type = "Deposit" },
                    new Transaction { Id = 2, Date = DateTime.UtcNow, Amount = -50, Type = "Withdrawal" }
                }
            };
            _repositoryMock.Setup(r => r.GetBankAccountAsync(1)).ReturnsAsync(account);

            // Act
            var result = await _service.GetStatementAsync(1);

            // Assert
            Assert.Equal("Bank Account", result.AccountType);
            Assert.Equal(100, result.Balance);
            Assert.Equal(2, result.Transactions.Count);
        }

        [Fact]
        public async Task CreateAccountAsync_ShouldAddNewAccount()
        {
            // Arrange
            var account = new BankAccount { Id = 1, AccountNumber = "1111", Balance = 100 };
            _repositoryMock.Setup(r => r.AddBankAccountAsync(account)).ReturnsAsync(account);

            // Act
            var result = await _service.CreateAccountAsync(account);

            // Assert
            Assert.Equal(account, result);
        }

        [Fact]
        public async Task CloseAccountAsync_ShouldRemoveAccount()
        {
            // Arrange
            var account = new BankAccount { Id = 1, AccountNumber = "1111", Balance = 100 };
            _repositoryMock.Setup(r => r.GetBankAccountAsync(1)).ReturnsAsync(account);
            _repositoryMock.Setup(r => r.DeleteBankAccountAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _service.CloseAccountAsync(1);

            // Assert
            _repositoryMock.Verify(r => r.DeleteBankAccountAsync(1), Times.Once);
        }

        [Fact]
        public async Task UpdateOverdraftAsync_ShouldUpdateOverdraft()
        {
            // Arrange
            var account = new BankAccount { Id = 1, AccountNumber = "1111", Balance = 100, HasOverdraft = false, OverdraftLimit = 0 };
            _repositoryMock.Setup(r => r.GetBankAccountAsync(1)).ReturnsAsync(account);

            // Act
            var result = await _service.UpdateOverdraftAsync(1, true, 200);

            // Assert
            Assert.True(result.HasOverdraft);
            Assert.Equal(200, result.OverdraftLimit);
        }

        [Fact]
        public async Task UpdateDepositLimitAsync_ShouldUpdateDepositLimit()
        {
            // Arrange
            var account = new SavingsAccount { Id = 1, AccountNumber = "1111", Balance = 100, DepositLimit = 500 };
            _repositoryMock.Setup(r => r.GetBankAccountAsync(1)).ReturnsAsync(account);

            // Act
            var result = await _service.UpdateDepositLimitAsync(1, 1000);

            // Assert
            Assert.Equal(1000, result.DepositLimit);
        }
    }
}