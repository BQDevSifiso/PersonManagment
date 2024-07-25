using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonManagment.Domain.Entities;

namespace PersonManagment.Infrustructure.Persistence
{
    public class PersonManagmentDbContext : IdentityDbContext
    {
        public PersonManagmentDbContext(DbContextOptions<PersonManagmentDbContext> options): base(options)
        {
            
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }      
    }
}
