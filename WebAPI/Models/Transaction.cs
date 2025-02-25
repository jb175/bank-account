namespace WebAPI.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // "Deposit" or "Withdrawal"
    }
}
