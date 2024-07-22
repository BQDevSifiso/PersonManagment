using Microsoft.EntityFrameworkCore;
using PersonManagment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonManagment.Infrustructure.Persistence
{
    public class PersonManagmentDbContext : DbContext
    {
        public PersonManagmentDbContext(DbContextOptions<PersonManagmentDbContext> options): base(options)
        {
            
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }        
    }
}
