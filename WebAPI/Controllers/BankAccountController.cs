using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _service;

        public BankAccountController(IBankAccountService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount(BankAccount account)
        {
            var createdAccount = await _service.CreateAccountAsync(account);
            return CreatedAtAction(nameof(GetStatement), new { accountId = createdAccount.Id }, createdAccount);
        }

        [HttpPost("create-savings")]
        public async Task<IActionResult> CreateSavingsAccount(SavingsAccount account)
        {
            var createdAccount = await _service.CreateSavingsAccountAsync(account);
            return CreatedAtAction(nameof(GetStatement), new { accountId = createdAccount.Id }, createdAccount);
        }

        [HttpDelete("close/{accountId}")]
        public async Task<IActionResult> CloseAccount(int accountId)
        {
            await _service.CloseAccountAsync(accountId);
            return Ok(new { Message = "Account closed successfully" });
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(int accountId, decimal amount)
        {
            var account = await _service.DepositAsync(accountId, amount);
            return Ok(account);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(int accountId, decimal amount)
        {
            var account = await _service.WithdrawAsync(accountId, amount);
            return Ok(account);
        }

        [HttpGet("statement")]
        public async Task<IActionResult> GetStatement(int accountId)
        {
            var statement = await _service.GetStatementAsync(accountId);
            return Ok(statement);
        }

        [HttpPut("update-overdraft")]
        public async Task<IActionResult> UpdateOverdraft(int accountId, bool hasOverdraft, decimal overdraftLimit)
        {
            var account = await _service.UpdateOverdraftAsync(accountId, hasOverdraft, overdraftLimit);
            return Ok(account);
        }

        [HttpPut("update-deposit-limit")]
        public async Task<IActionResult> UpdateDepositLimit(int accountId, decimal depositLimit)
        {
            var account = await _service.UpdateDepositLimitAsync(accountId, depositLimit);
            return Ok(account);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _service.GetAllAccountsAsync();
            var result = accounts.Select<BankAccount, object>(account =>
            {
                if (account is SavingsAccount savingsAccount)
                {
                    return new
                    {
                        account.Id,
                        account.AccountNumber,
                        account.Balance,
                        DepositLimit = savingsAccount.DepositLimit
                    };
                }
                else
                {
                    return new
                    {
                        account.Id,
                        account.AccountNumber,
                        account.Balance,
                        account.HasOverdraft,
                        account.OverdraftLimit
                    };
                }
            });

            return Ok(result);
        }
    }
}
