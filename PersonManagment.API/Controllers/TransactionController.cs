using Microsoft.AspNetCore.Mvc;
using PersonManagment.API.Filters.ActionFilters;
using PersonManagment.API.Filters.AuthFilters;
using PersonManagment.Application.Services;
using PersonManagment.Domain.Entities;

namespace PersonManagment.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [JwtTokenAuthFilter]
    public class TransactionController: ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly AccountService _accountService;
        public TransactionController(TransactionService transactionService, AccountService accountService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
        }
        // GET: api/Transaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return Ok(await _transactionService.GetTransactionsAsync());
        }

        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        // POST: api/Transaction
        [HttpPost]
        [TypeFilter(typeof(Transaction_ValidateCreateTransactionFilterAttribute))]
        public async Task<ActionResult<Transaction>> AddTransaction(Transaction transaction)
        {
            var createdTransaction = await _transactionService.CreateTransactionAsync(transaction);

            //update account outstanding balance on new transaction
            var isUpdate = await _accountService.UpdateAccountBalanceOnNewTransaction(createdTransaction.AccountId, createdTransaction.Amount);

            return CreatedAtAction(nameof(GetTransaction), new { id = createdTransaction.TransactionId }, createdTransaction);
        }

        // PUT: api/Transaction/5
        [HttpPut("{id}")]
        [TypeFilter(typeof(Transaction_ValidatedTransactionDateUnchangedFilterAttribute))]
        public async Task<IActionResult> UpdateTransaction(int id, Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return BadRequest();
            }
            var existingTransaction = await _transactionService.GetTransactionByIdAsync(id);
            await _transactionService.UpdateTransactionAsync(transaction);

            //update account outstanding balance on existing transaction
            var isUpdated = await _accountService.UpdateAccountBalanceOnExistingTransaction(transaction.AccountId,existingTransaction.Amount, transaction.Amount);

            return NoContent();
        }

        // DELETE: api/Transaction/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            await _transactionService.DeleteTransactionAsync(id);
            return NoContent();
        }
    }
}
