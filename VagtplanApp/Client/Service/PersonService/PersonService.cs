using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VagtplanApp.Shared.Model;
using static System.Net.WebRequestMethods;

namespace VagtplanApp.Client.Services
{
    public class PersonService : IPersonService
    {
        private readonly HttpClient httpClient;
       // private Person _currentUser;

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

        public async Task<Person> Authenticate(string email, string password)
        {
            var loginPerson = new Person { Email = email, Password = password };
            var response = await httpClient.PostAsJsonAsync("api/persons/authenticate", loginPerson);

            if (response.IsSuccessStatusCode)
            {
                CurrentUser = await response.Content.ReadFromJsonAsync<Person>();
                return CurrentUser;
            }
            else
            {
                return null;
            }
        }
        public bool IsUserLoggedIn()
        {
            var loggedIn = CurrentUser != null;
            Console.WriteLine($"Er bruger logget ind: {loggedIn}"); // tjekker i konsol om bruger er logget ind
            return loggedIn;
        }

    }
}
