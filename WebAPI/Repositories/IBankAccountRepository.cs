using WebAPI.Models;

namespace WebAPI.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> GetBankAccountAsync(int accountId);
        Task<BankAccount> AddBankAccountAsync(BankAccount bankAccount);
        Task<BankAccount> UpdateBankAccountAsync(BankAccount bankAccount);
        Task DeleteBankAccountAsync(int accountId);
        Task AddTransactionAsync(Transaction transaction);
        Task<IEnumerable<BankAccount>> GetAllAccountsAsync();
    }
}
