using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using VagtplanApp.Shared.Model;
using static System.Net.WebRequestMethods;

namespace VagtplanApp.Client.Services
{
    public class PersonService : IPersonService
    {
        private readonly HttpClient httpClient;
       // private Person _currentUser;

        public Person CurrentUser { get; private set; }
        private readonly ILocalStorageService localStore; // Tilføjer localStore for at gemme currentUser lokalt i browseren


        public PersonService(HttpClient httpClient, ILocalStorageService localStore)
        {
            this.httpClient = httpClient;
            this.localStore = localStore;
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

                // Gemmer den nuværende bruger i local storage for at opretholde tilstanden
                await localStore.SetItemAsync("currentUser", CurrentUser);

                return CurrentUser;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IsUserLoggedInAsync()
        {
            // Forsøger at gendanne bruger fra local storage hvis CurrentUser er null
            if (CurrentUser == null)
            {
                CurrentUser = await localStore.GetItemAsync<Person>("currentUser");
            }
            return CurrentUser != null;
        }
    }
}
