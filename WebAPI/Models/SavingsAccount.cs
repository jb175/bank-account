namespace WebAPI.Models
{
    public class SavingsAccount : BankAccount
    {
        public decimal DepositLimit { get; set; }

        // Override properties to ensure SavingsAccount cannot have an overdraft
        public new bool HasOverdraft { get => false; set { } }
        public new decimal OverdraftLimit { get => 0; set { } }
    }
}
