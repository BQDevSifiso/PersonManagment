using Moq;
using Xunit;
using PersonManagment.Application.Services;
using PersonManagment.Domain.Entities;
using PersonManagment.Domain.Interfaces;

namespace PersonManagment.Tests.Services
{
    public class PersonServiceTests
    {
        private readonly Mock<IRepository<Person>> _personRepositoryMock;
        private readonly PersonService _personService;
        public PersonServiceTests()
        {
            _personRepositoryMock = new Mock<IRepository<Person>>();
            _personService = new PersonService(_personRepositoryMock.Object);
        }

        [Fact]
        public async Task GetPersonsAsync_ReturnsAllPersons()
        {
            // Arrange
            var persons = new List<Person> { new Person(), new Person() };
            _personRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(persons);

            // Act
            var result = await _personService.GetPersonsAsync();

            // Assert
            Assert.Equal(persons.Count, result.Count());
            _personRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ReturnsPerson()
        {
            // Arrange
            var person = new Person { PersonId = 1 };
            _personRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(person);

            // Act
            var result = await _personService.GetPersonByIdAsync(1);

            // Assert
            Assert.Equal(person.PersonId, result.PersonId);
            _personRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task CreatePersonAsync_AddsPerson()
        {
            // Arrange
            var person = new Person { PersonId = 1 };
            _personRepositoryMock.Setup(repo => repo.AddAsync(person)).ReturnsAsync(person);

            // Act
            var result = await _personService.CreatePersonAsync(person);

            // Assert
            Assert.Equal(person.PersonId, result.PersonId);
            _personRepositoryMock.Verify(repo => repo.AddAsync(person), Times.Once);
        }

        [Fact]
        public async Task UpdatePersonAsync_UpdatesPerson()
        {
            // Arrange
            var person = new Person { PersonId = 1 };

            // Act
            await _personService.UpdatePersonAsync(person);

            // Assert
            _personRepositoryMock.Verify(repo => repo.UpdateAsync(person), Times.Once);
        }

        [Fact]
        public async Task DeletePersonAsync_DeletesPerson()
        {
            // Arrange
            var personId = 3;

            // Act
            await _personService.DeletePersonAsync(personId);

            // Assert
            _personRepositoryMock.Verify(repo => repo.DeleteAsync(personId), Times.Once);
        }

    }
}
