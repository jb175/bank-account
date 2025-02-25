using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _repository;

        public BankAccountService(IBankAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<BankAccount> CreateAccountAsync(BankAccount account)
        {
            return await _repository.AddBankAccountAsync(account);
        }

        public async Task<SavingsAccount> CreateSavingsAccountAsync(SavingsAccount account)
        {
            return (SavingsAccount)await _repository.AddBankAccountAsync(account);
        }

        public async Task CloseAccountAsync(int accountId)
        {
            var account = await _repository.GetBankAccountAsync(accountId);
            if (account == null) throw new Exception("Account not found");

            await _repository.DeleteBankAccountAsync(accountId);
        }

        public async Task<BankAccount> DepositAsync(int accountId, decimal amount)
        {
            var account = await _repository.GetBankAccountAsync(accountId);
            if (account == null) throw new Exception("Account not found");

            if (account is SavingsAccount savingsAccount && account.Balance + amount > savingsAccount.DepositLimit)
            {
                throw new Exception("Deposit limit exceeded");
            }

            account.Balance += amount;
            await _repository.UpdateBankAccountAsync(account);
            await _repository.AddTransactionAsync(new Transaction
            {
                BankAccountId = accountId,
                Date = DateTime.UtcNow,
                Amount = amount,
                Type = "Deposit"
            });

            return account;
        }

        public async Task<BankAccount> WithdrawAsync(int accountId, decimal amount)
        {
            var account = await _repository.GetBankAccountAsync(accountId);
            if (account == null) throw new Exception("Account not found");

            if (account.HasOverdraft)
            {
                if (account.Balance - amount < -account.OverdraftLimit)
                {
                    throw new Exception("Insufficient funds or overdraft limit");
                }
            }
            else
            {
                if (account.Balance < amount)
                {
                    throw new Exception("Insufficient funds");
                }
            }

            account.Balance -= amount;
            await _repository.UpdateBankAccountAsync(account);
            await _repository.AddTransactionAsync(new Transaction
            {
                BankAccountId = accountId,
                Date = DateTime.UtcNow,
                Amount = -amount,
                Type = "Withdrawal"
            });

            return account;
        }

        public async Task<AccountStatementDto> GetStatementAsync(int accountId)
        {
            var account = await _repository.GetBankAccountAsync(accountId);
            if (account == null) throw new Exception("Account not found");

            var statement = new AccountStatementDto
            {
                AccountType = account is SavingsAccount ? "Savings Account" : "Bank Account",
                Balance = account.Balance,
                Transactions = account.Transactions.OrderByDescending(t => t.Date)
                    .Select(t => new TransactionDto
                    {
                        Id = t.Id,
                        Date = t.Date,
                        Amount = t.Amount,
                        Type = t.Type
                    }).ToList()
            };

            return statement;
        }

        public async Task<BankAccount> UpdateOverdraftAsync(int accountId, bool hasOverdraft, decimal overdraftLimit)
        {
            var account = await _repository.GetBankAccountAsync(accountId);
            if (account == null) throw new Exception("Account not found");

            if (account is SavingsAccount)
            {
                throw new Exception("Savings accounts cannot have an overdraft");
            }

            account.HasOverdraft = hasOverdraft;
            account.OverdraftLimit = overdraftLimit;
            await _repository.UpdateBankAccountAsync(account);

            return account;
        }

        public async Task<SavingsAccount> UpdateDepositLimitAsync(int accountId, decimal depositLimit)
        {
            var account = await _repository.GetBankAccountAsync(accountId) as SavingsAccount;
            if (account == null) throw new Exception("Savings account not found");

            account.DepositLimit = depositLimit;
            await _repository.UpdateBankAccountAsync(account);

            return account;
        }

        public async Task<IEnumerable<BankAccount>> GetAllAccountsAsync()
        {
            return await _repository.GetAllAccountsAsync();
        }
    }
}
