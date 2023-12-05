using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories
{
    public interface IPersonRepository
    {
        List<Person> GetAllPersons();
        Task CreatePerson(Person person);
        Task<Person> GetPersonByEmail(string email);
        Task UpdatePerson(Person updatePerson);
    }
}
