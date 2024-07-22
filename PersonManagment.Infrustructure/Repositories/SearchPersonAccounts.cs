using Microsoft.EntityFrameworkCore;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;
using PersonManagment.Infrustructure.Persistence;

namespace TVA.Infrastructure.Repositories
{
    public class SearchPersonAccounts : ISearchPersonAccounts
    {
        private readonly PersonManagmentDbContext _context;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Account> _accountRepository;

        public SearchPersonAccounts(PersonManagmentDbContext context, IRepository<Person> personRepository, IRepository<Account> accountRepository)
        {
            _context = context;
            _personRepository = personRepository;
            _accountRepository = accountRepository;
        }

        public async Task<List<Account>> GetPersonAccountsByPersonId(int personId)
        {
            var accounts = await _context.Accounts.AsNoTracking().Where(a => a.PersonId == personId).ToListAsync();
            if (accounts.Count == 0)
            {
                accounts = null;
            }
            return accounts;
        }

        public async Task<IEnumerable<Person>> FindPersonOrAccount(string filterString)
        {
            var persons = await _personRepository.GetAllAsync();
            var accounts = await _accountRepository.GetAllAsync();
            var personList = new List<Person>();
            var accountList = new List<Account>();

            if (!string.IsNullOrWhiteSpace(filterString))
            {
                personList = persons.Where(p => p.IDNumber.Equals(filterString, StringComparison.OrdinalIgnoreCase) || p.LastName.Equals(filterString, StringComparison.OrdinalIgnoreCase)).ToList();

                if (personList.Count > 0)
                {
                    foreach (var person in personList)
                    {
                        accountList = accounts.Where(a => a.PersonId == person.PersonId).OrderByDescending(c=> c.CreatedDate).ToList();
                        person.Accounts = accountList;
                    }
                }
                else
                {
                    var accountPerson = accounts.Where(a => a.AccountNumber.Equals(filterString, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (accountPerson != null)
                    {
                        accountList.Add(accountPerson);
                        personList = persons.Where(p => p.PersonId == accountPerson.PersonId).ToList();
                        foreach (var person in personList)
                        {
                            person.Accounts = accountList;
                        }
                    }
                }                
            }  

            return personList;
        }
        
    }

}
