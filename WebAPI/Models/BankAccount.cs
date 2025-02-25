namespace WebAPI.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public bool HasOverdraft { get; set; }
        public decimal OverdraftLimit { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
