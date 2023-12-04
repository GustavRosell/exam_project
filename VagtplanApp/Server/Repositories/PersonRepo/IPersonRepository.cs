using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories
{
    public interface IPersonRepository
    {
        List<Person> GetAll();
        Task AddPerson(Person person);
        Task<Person> GetPersonByEmail(string email);
        //Task<Person> GetLatestPerson();
    }
}
