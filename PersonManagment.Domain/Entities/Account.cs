using System.ComponentModel.DataAnnotations;

namespace PersonManagment.Domain.Entities
{
    public class Account
    {
        public int AccountId { get; set; }

        [Required]
        [MaxLength(20)]
        public string AccountNumber { get; set; }

        [Required]
        public int PersonId { get; set; } = 0;
        public Person? Person { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Balance { get; set; }

        [Required]
        public bool IsClosed { get; set; } = false;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ClosedDate { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}
