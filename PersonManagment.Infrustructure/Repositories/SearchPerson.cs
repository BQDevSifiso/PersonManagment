using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;
using PersonManagment.Infrustructure.Persistence;

namespace TVA.Infrastructure.Repositories
{
    public class SearchPerson : ISearchPerson
    {
        private readonly PersonManagmentDbContext _context;

        public SearchPerson(PersonManagmentDbContext context)
        {
            _context = context;
        }
        public async Task<Person> FindPersonByIDNumber(string idNumber)
        {
            var person = await _context.Persons.Where(p => p.IDNumber == idNumber).FirstOrDefaultAsync();
            return person;
        }
    }
}
