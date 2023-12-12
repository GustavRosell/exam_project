using VagtplanApp.Shared.Model;

namespace VagtplanApp.Client.Services
{
    public interface IPersonService
    {
        Person CurrentUser { get; }
        Task<bool> AddPerson(Person person);
        void SetCurrentUser(Person user);
        bool IsKoordinator();
        Task<Person> Authenticate(string email, string password);
        Task<bool> IsUserLoggedInAsync();
        Task LogOut();
        Task UpdatePerson(Person updatedPerson);
    }
}
