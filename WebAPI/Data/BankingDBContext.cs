using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions<BankingDbContext> options)
        : base(options)
    {
    }

    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<SavingsAccount> SavingsAccounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}