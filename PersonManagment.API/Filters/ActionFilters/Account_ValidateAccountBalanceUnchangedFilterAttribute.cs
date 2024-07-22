using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PersonManagment.Application.Services;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;
using PersonManagment.Infrastructure.Repositories;

namespace PersonManagment.API.Filters.ActionFilters
{
    public class Account_ValidateAccountBalanceUnchangedFilterAttribute : ActionFilterAttribute
    {
        private readonly ISearchAccount _searchAccount;
        public Account_ValidateAccountBalanceUnchangedFilterAttribute(ISearchAccount searchAccount)
        {
            _searchAccount = searchAccount;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var account = context.ActionArguments["account"] as Account;
            if (account != null)
            {
                var existAccount = _searchAccount.GetAccountByAccountId(account.AccountId);
                if (account.Balance != existAccount.Result?.Balance)
                {
                    context.ModelState.AddModelError("Account", $"Cannot change the account's outstanding balance!");
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
