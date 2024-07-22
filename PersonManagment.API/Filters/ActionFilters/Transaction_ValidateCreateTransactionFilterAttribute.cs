using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PersonManagment.Application.Services;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;

namespace PersonManagment.API.Filters.ActionFilters
{
    public class Transaction_ValidateCreateTransactionFilterAttribute : ActionFilterAttribute
    {
        private readonly ISearchAccount _searchAccount;

        public Transaction_ValidateCreateTransactionFilterAttribute(ISearchTransaction searchTransaction, ISearchAccount searchAccount)
        {
            _searchAccount = searchAccount;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var transaction = context.ActionArguments["transaction"] as Transaction;

            if (transaction == null)
            {
                context.ModelState.AddModelError("Transaction", "Transaction object is null");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {

                if (transaction.AccountId == 0)
                {
                    context.ModelState.AddModelError("Transaction", $"AccountId cannot be equal to zero");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else
                {
                    var account = _searchAccount.GetAccountByAccountId(transaction.AccountId);
                    if (account.Result != null)
                    {
                        if (account.Result.IsClosed)
                        {
                            context.ModelState.AddModelError("Transaction", $"Cannot add transaction to a closed account");
                            var problemDetails = new ValidationProblemDetails(context.ModelState)
                            {
                                Status = StatusCodes.Status400BadRequest
                            };
                            context.Result = new BadRequestObjectResult(problemDetails);
                        }
                    }
                    else
                    {
                        context.ModelState.AddModelError("Transaction", $"Account does not exist, Transaction can only be added to an existing account");
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
}
