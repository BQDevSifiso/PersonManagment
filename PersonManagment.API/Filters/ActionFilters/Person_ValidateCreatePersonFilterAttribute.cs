using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;

namespace PersonManagment.API.Filters.ActionFilters
{
    public class Person_ValidateCreatePersonFilterAttribute : ActionFilterAttribute
    {
        private readonly ISearchPersonAccounts _searchPersonAccounts;
        private readonly ISearchPerson _searchPerson;

        public Person_ValidateCreatePersonFilterAttribute(ISearchPersonAccounts searchPersonAccounts, ISearchPerson searchPerson)
        {
            _searchPersonAccounts = searchPersonAccounts;
            _searchPerson = searchPerson;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var person = context.ActionArguments["person"] as Person;

            if (person == null)
            {
                context.ModelState.AddModelError("Person", "Person Object is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                var existingPerson = _searchPerson.FindPersonByIDNumber(person.IDNumber);
                if(existingPerson.Result != null)
                {
                    context.ModelState.AddModelError("Person", $"Person with ID number: {person.IDNumber} already Exist!");
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
