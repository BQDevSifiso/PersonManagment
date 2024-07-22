using Microsoft.AspNetCore.Mvc;
using PersonManagment.API.Filters.ActionFilters;
using PersonManagment.Application.Services;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;

namespace PersonManagment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PersonController: ControllerBase
    {
        private readonly PersonService _personService;
        private readonly ISearchPersonAccounts _personAccounts;
        public PersonController(PersonService personService, ISearchPersonAccounts personAccounts)
        {
            _personService = personService;
            _personAccounts = personAccounts;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return Ok(await _personService.GetPersonsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        [HttpPost]
        [TypeFilter(typeof(Person_ValidateCreatePersonFilterAttribute))]
        public async Task<ActionResult<Person>> CreatePerson([FromBody] Person person)
        {
            var createdPerson = await _personService.CreatePersonAsync(person);
            return CreatedAtAction(nameof(GetPerson), new { id = createdPerson.PersonId }, createdPerson);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }

            await _personService.UpdatePersonAsync(person);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(Person_ValidateDeletePersonFilterAttribute))]
        public async Task<IActionResult> DeletePerson(int id)
        {
            await _personService.DeletePersonAsync(id);
            return NoContent();
        }

        [HttpGet("{filterString}")]
        public async Task<IActionResult> FilterPersonAccount(string filterString)
        {
            var personAccounts = await _personAccounts.FindPersonOrAccount(filterString);
            return Ok(personAccounts);

        }
    }
}
