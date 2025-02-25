using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IBankAccountService
    {
        Task<BankAccount> CreateAccountAsync(BankAccount account);
        Task<SavingsAccount> CreateSavingsAccountAsync(SavingsAccount account);
        Task CloseAccountAsync(int accountId);
        Task<BankAccount> DepositAsync(int accountId, decimal amount);
        Task<BankAccount> WithdrawAsync(int accountId, decimal amount);
        Task<AccountStatementDto> GetStatementAsync(int accountId);
        Task<BankAccount> UpdateOverdraftAsync(int accountId, bool hasOverdraft, decimal overdraftLimit);
        Task<SavingsAccount> UpdateDepositLimitAsync(int accountId, decimal depositLimit);
        Task<IEnumerable<BankAccount>> GetAllAccountsAsync();
    }
}
