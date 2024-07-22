using PersonManagment.Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace PersonManagment.Domain.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        [Required]
        public int AccountId { get; set; }
        public Account? Account { get; set; }        

        [Required]
        [Transaction_EnsureAmountNotZero]
        public decimal Amount { get; set; }

        [Required]
        [Transaction_EnsureNotFutureDate]
        public DateTime TransactionDate { get; set; }       
    }
}
