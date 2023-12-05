using System.Threading.Tasks;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Client.Services
{
    public interface IPersonService
    {
        Person CurrentUser { get; }

        Task<bool> AddPerson(Person person);
        void SetCurrentUser(Person user);
        bool IsKoordinator();
        
        // Metode til at autentificere en bruger og returnere brugerobjektet hvis gyldigt, ellers null
        Task<Person> Authenticate(string email, string password);
        Task<bool> IsUserLoggedInAsync();
        Task LogOut();
        Task<bool> UpdateCurrentUser(Person updatedPerson);

    }
}
