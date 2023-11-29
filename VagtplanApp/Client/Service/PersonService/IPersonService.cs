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
        Task<bool> Login(string email, string password);
        bool IsUserLoggedIn(); 
        Task Logout(); 
    }
}
