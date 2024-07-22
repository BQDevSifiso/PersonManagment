using System.ComponentModel.DataAnnotations;

namespace PersonManagment.Domain.Entities
{
    public class Person
    {
        public int PersonId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(20)]
        public string IDNumber { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }       

        public ICollection<Account>? Accounts { get; set; }
    }
}
