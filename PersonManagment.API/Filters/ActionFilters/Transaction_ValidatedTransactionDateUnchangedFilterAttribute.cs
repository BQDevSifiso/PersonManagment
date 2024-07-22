using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;

namespace PersonManagment.API.Filters.ActionFilters
{
    public class Transaction_ValidatedTransactionDateUnchangedFilterAttribute : ActionFilterAttribute
    {
        private readonly ISearchTransaction _searchTransaction;
        public Transaction_ValidatedTransactionDateUnchangedFilterAttribute(ISearchTransaction searchTransaction)
        {
            _searchTransaction = searchTransaction;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var transaction = context.ActionArguments["transaction"] as Transaction;
            if (transaction != null)
            {
                var existingTransaction = _searchTransaction.GetTransactionById(transaction.TransactionId);
                if (transaction.TransactionDate != existingTransaction.Result.TransactionDate)
                {
                    context.ModelState.AddModelError("Transaction", $"Cannot change the transaction date!");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
            }
        }
    }
}
