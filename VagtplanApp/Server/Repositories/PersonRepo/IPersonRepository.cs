using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories
{
    public interface IPersonRepository
    {
        List<Person> GetAll();
        Task AddPerson(Person person);

        // Tilføj senere f.eks: slet person
    }
}