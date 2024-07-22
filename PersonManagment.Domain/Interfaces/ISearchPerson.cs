using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonManagment.Domain.Entities;

namespace PersonManagment.Domain.Interfaces
{
    public interface ISearchPerson
    {
        Task<Person> FindPersonByIDNumber(string idNumber);
    }
}
