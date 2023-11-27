using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories
{
    public interface IPersonRepository
    {
        List<Person> GetAll();
    }
}