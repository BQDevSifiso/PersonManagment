using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonManagment.Domain.Entities;

namespace PersonManagment.Domain.Validations
{
    internal class Transaction_EnsureAmountNotZeroAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var transaction = validationContext.ObjectInstance as Transaction;
            if (transaction != null)
            {
                if (transaction.Amount == 0.00m)
                {
                    return new ValidationResult("Transaction Amount cannot be zero");
                }
            }
            return ValidationResult.Success;
        }
    }
}
