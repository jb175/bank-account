using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly BankingDbContext _context;

        public BankAccountRepository(BankingDbContext context)
        {
            _context = context;
        }

        public async Task<BankAccount> GetBankAccountAsync(int accountId)
        {
            return await _context.BankAccounts.Include(a => a.Transactions).FirstOrDefaultAsync(a => a.Id == accountId);
        }

        public async Task<BankAccount> AddBankAccountAsync(BankAccount bankAccount)
        {
            _context.BankAccounts.Add(bankAccount);
            await _context.SaveChangesAsync();
            return bankAccount;
        }

        public async Task<BankAccount> UpdateBankAccountAsync(BankAccount bankAccount)
        {
            _context.Entry(bankAccount).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return bankAccount;
        }

        public async Task DeleteBankAccountAsync(int accountId)
        {
            var account = await _context.BankAccounts.FindAsync(accountId);
            if (account != null)
            {
                _context.BankAccounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BankAccount>> GetAllAccountsAsync()
        {
            return await _context.BankAccounts.Include(a => a.Transactions).ToListAsync();
        }
    }
}
