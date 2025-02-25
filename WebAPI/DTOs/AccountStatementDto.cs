namespace WebAPI.DTOs
{
    public class AccountStatementDto
    {
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }
}
