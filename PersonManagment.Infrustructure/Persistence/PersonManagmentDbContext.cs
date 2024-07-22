using Microsoft.EntityFrameworkCore;
using PersonManagment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonManagment.Infrustructure.Persistence
{
    internal class PersonManagmentDbContext : DbContext
    {
        public PersonManagmentDbContext(DbContextOptions<PersonManagmentDbContext> options): base(options)
        {
            
        }
        internal DbSet<Person> Persons { get; set; }
        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<Transaction> Transactions { get; set; }        
    }
}
