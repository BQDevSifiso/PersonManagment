using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonManagment.Domain.Entities;

namespace PersonManagment.Domain.Interfaces
{
    public interface ISearchPersonAccounts
    {
        Task<List<Account>> GetPersonAccountsByPersonId(int personId);

        Task<IEnumerable<Person>> FindPersonOrAccount(string filterString);
    }
}
