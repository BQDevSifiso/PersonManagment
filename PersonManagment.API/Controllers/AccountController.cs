using Microsoft.AspNetCore.Mvc;
using PersonManagment.API.Filters.ActionFilters;
using PersonManagment.Application.Services;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;

namespace PersonManagment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly ISearchTransaction _searchTransaction;
        private readonly ISearchAccount _searchAccount;

        public AccountController(AccountService accountService, ISearchTransaction searchTransaction, ISearchAccount searchAccount)
        {
            _accountService = accountService;
            _searchTransaction = searchTransaction;
            _searchAccount = searchAccount;
        }

        // GET: api/Account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return Ok(await _accountService.GetAccountsAsync());
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        [TypeFilter(typeof(Account_ValidateAccountExistsFilterAttribute))]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _searchAccount.GetAccountByAccountId(id);
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
        [TypeFilter(typeof(Account_ValidateAccountExistsFilterAttribute))]
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
        [HttpPut("{id}")]
        [TypeFilter(typeof(Account_ValidateAccountExistsFilterAttribute))]
        public async Task<IActionResult> CloseAccount(int id)
        {
            var isClosed = await _accountService.CloseAccount(id);

            if (isClosed == false)
            {
                ModelState.AddModelError("Invalid", "Accounts cannot be closed if the balance is not zero");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                return new UnauthorizedObjectResult(problemDetails);
            }
            return Ok(isClosed);
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        [TypeFilter(typeof(Account_ValidateAccountExistsFilterAttribute))]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            await _accountService.DeleteAccountAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Account_ValidateAccountExistsFilterAttribute))]
        public async Task<IActionResult> GetAccountTransactions(int id)
        {
            var trasactions = await _searchTransaction.GetTransactionsByAccoutId(id);
            return Ok(trasactions);
        }
    }
}
