using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PersonManagment.Domain.Interfaces;

namespace PersonManagment.API.Filters.ActionFilters
{
    public class Account_ValidateAccountExistsFilterAttribute : ActionFilterAttribute
    {
        private readonly ISearchAccount _searchAccount;
        public Account_ValidateAccountExistsFilterAttribute(ISearchAccount searchAccount)
        {
            _searchAccount = searchAccount;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var accountId = context.ActionArguments["id"] as int?;
            if (accountId != null)
            {
                if (_searchAccount.GetAccountByAccountId((int)accountId).Result == null)
                {
                    context.ModelState.AddModelError("Account", "Account does not exist.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };

                    context.Result = new NotFoundObjectResult(problemDetails);
                }
            }

        }
    }
}
