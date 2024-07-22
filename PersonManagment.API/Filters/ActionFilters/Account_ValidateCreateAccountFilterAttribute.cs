using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using PersonManagment.Application.Services;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;

namespace PersonManagment.API.Filters.ActionFilters
{
    public class Account_ValidateCreateAccountFilterAttribute : ActionFilterAttribute
    {
        private readonly ISearchAccount _searchAccount;
        private readonly PersonService _searchPerson;
        public Account_ValidateCreateAccountFilterAttribute(ISearchAccount searchAccount, PersonService searchPerson)
        {
            _searchAccount = searchAccount;
            _searchPerson = searchPerson;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var account = context.ActionArguments["account"] as Account;

            if (account == null)
            {
                context.ModelState.AddModelError("Account", "Account object is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                var existingAccount = _searchAccount.GetAccountByAccountNumber(account.AccountNumber);
                if (existingAccount.Result != null)
                {
                    context.ModelState.AddModelError("Account", $"Account with Account Number: {account.AccountNumber} already Exist!");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else
                {
                    if (account.PersonId == 0)
                    {
                        context.ModelState.AddModelError("Account", $"PersonId cannot be equal to zero");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);
                    }
                    else
                    {
                        var person = _searchPerson.GetPersonByIdAsync(account.PersonId);
                        if (person.Result == null)
                        {
                            context.ModelState.AddModelError("Account", $"Person does not exist, New accounts can only be added after the person is created");
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
}
