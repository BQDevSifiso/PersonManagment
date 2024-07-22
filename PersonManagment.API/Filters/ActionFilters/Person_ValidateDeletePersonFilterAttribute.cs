using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PersonManagment.Domain.Interfaces;

namespace PersonManagment.API.Filters.ActionFilters
{
    public class Person_ValidateDeletePersonFilterAttribute : ActionFilterAttribute
    {
        private readonly ISearchPersonAccounts _searchPersonAccounts;

        public Person_ValidateDeletePersonFilterAttribute(ISearchPersonAccounts searchPersonAccounts)
        {
            _searchPersonAccounts = searchPersonAccounts;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var personId = context.ActionArguments["id"] as int?;
            if (personId.HasValue)
            {
                if (personId.Value <= 0)
                {
                    context.ModelState.AddModelError("PersonId", "PersonId is not valid");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else
                {
                    var accounts = _searchPersonAccounts.GetPersonAccountsByPersonId(personId.Value);
                    if (accounts.Result != null)
                    {
                        var openedAccounts = accounts.Result.Where(a => a.IsClosed == false).ToList();
                        if (openedAccounts.Count > 0)
                        {
                            context.ModelState.AddModelError("PersonId", "Cannot delete Person with active accounts");
                            var problemDetails = new ValidationProblemDetails(context.ModelState)
                            {
                                Status = StatusCodes.Status403Forbidden
                            };
                            context.Result = new BadRequestObjectResult(problemDetails);
                        }                        
                    }
                }
            }
        }

    }
}
