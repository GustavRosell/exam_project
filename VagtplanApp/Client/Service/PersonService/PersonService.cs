using System.Net.Http.Json;
using Blazored.LocalStorage;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Client.Services
{
    public class PersonService : IPersonService
    {
        private readonly HttpClient httpClient; // HTTP klient bruges til at lave web requests
        private readonly ILocalStorageService localStore; // Til at gemme og hente brugerdata fra lokal storage

        public Person CurrentUser { get; private set; } // Holder styr på den nuværende bruger

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

        // 
        public bool IsKoordinator()
        {
            return CurrentUser != null && CurrentUser.isKoordinator;
        }

        // Metode for log in
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
            // Tjekker om brugeren er logget ind ved først at se på CurrentUser og derefter i localStorage
            if (CurrentUser == null)
            {
                CurrentUser = await localStore.GetItemAsync<Person>("currentUser");
            }
            return CurrentUser != null;
        }
    }
}
