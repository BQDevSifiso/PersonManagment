using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonManagment.Domain.Entities;

namespace PersonManagment.Domain.Validations
{
    public class Transaction_EnsureNotFutureDateAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var transaction = validationContext.ObjectInstance as Transaction;
            if (transaction != null)
            {
                if (transaction.TransactionDate > DateTime.UtcNow)
                {
                    return new ValidationResult("Transaction cannot have a future date");
                }
            }

            return ValidationResult.Success;
        }
    }
}
