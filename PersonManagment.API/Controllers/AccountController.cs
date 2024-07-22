using Microsoft.AspNetCore.Mvc;
using PersonManagment.API.Filters.ActionFilters;
using PersonManagment.Application.Services;
using PersonManagment.Domain.Entities;

namespace PersonManagment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController: ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/Account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return Ok(await _accountService.GetAccountsAsync());
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        // POST: api/Account
        [HttpPost]
        [TypeFilter(typeof(Account_ValidateCreateAccountFilterAttribute))]
        public async Task<ActionResult<Account>> CreateAccount(Account account)
        {
            var createdAccount = await _accountService.CreateAccountAsync(account);
            return CreatedAtAction(nameof(GetAccount), new { id = createdAccount.AccountId }, createdAccount);
        }

        // PUT: api/Account/5
        [HttpPut("{id}")]
        [TypeFilter(typeof(Account_ValidateAccountBalanceUnchangedFilterAttribute))]
        public async Task<IActionResult> UpdateAccount(int id, Account account)
        {
            if (id != account.AccountId)
            {
                return BadRequest();
            }

            await _accountService.UpdateAccountAsync(account);

            return NoContent();
        }
        [HttpPut("{accountId}")]
        public async Task<IActionResult> CloseAccount(int accountId)
        {
            var isClosed = await _accountService.CloseAccount(accountId);
            return NoContent();
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            await _accountService.DeleteAccountAsync(id);
            return NoContent();
        }
    }
}
