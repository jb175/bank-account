namespace WebAPI.Models
{
    public class AccountStatement
    {
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
