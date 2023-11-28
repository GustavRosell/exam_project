using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Client.Services
{
    public class PersonService : IPersonService
    {
        private readonly HttpClient httpClient;

        public Person CurrentUser { get; private set; }

        public PersonService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> AddPerson(Person person)
        {
            var response = await httpClient.PostAsJsonAsync("/api/persons/add", person);
            return response.IsSuccessStatusCode;
        }

        public void SetCurrentUser(Person user)
        {
            CurrentUser = user;
        }

        public bool IsKoordinator()
        {
            return CurrentUser != null && CurrentUser.isKoordinator;
        }

        // Andre nødvendige metoder
    }
}
