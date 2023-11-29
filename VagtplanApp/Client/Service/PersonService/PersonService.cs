using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using VagtplanApp.Shared.Model;
using Blazored.LocalStorage;
using System.Text.Json;

namespace VagtplanApp.Client.Services
{
    public class PersonService : IPersonService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStore; // til LocalStorage --> også tilføjet i konstruktøren

        public Person? CurrentUser { get; private set; }

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

        // CurrentUser bruges i NavMenu til at fremvise pages for brugerrolle som er logget ind 
        public void SetCurrentUser(Person user)
        {
            CurrentUser = user;
            Console.WriteLine($"Bruger logget ind: {user.Email}"); // tjekker i konsol om login virker + ift vise navMenu bruges CurrentUser 
        }

        public bool IsKoordinator()
        {
            return CurrentUser != null && CurrentUser.isKoordinator;
        }


        public async Task<bool> Login(string email, string password)
        {
            var loginPerson = new Person { Email = email, Password = password };
            var response = await httpClient.PostAsJsonAsync("/api/persons/login", loginPerson);
            if (response.IsSuccessStatusCode)
            {
                // Use JsonDocument to manually parse the JSON
                using var jsonDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var root = jsonDoc.RootElement;

                // Make sure to use the correct casing as per the JSON response
                var userEmail = root.GetProperty("email").GetString();
                var isCoordinator = root.GetProperty("isKoordinator").GetBoolean();

                await localStore.SetItemAsync("currentUserEmail", userEmail);
                await localStore.SetItemAsync("isKoordinator", isCoordinator);

                SetCurrentUser(new Person { Email = userEmail, isKoordinator = isCoordinator });
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsUserLoggedIn()
        {
            var loggedIn = CurrentUser != null;
            Console.WriteLine($"Er bruger logget ind: {loggedIn}"); // tjekker i konsol om bruger er logget ind
            return loggedIn;
        }

        public async Task Logout()
        {
            CurrentUser = null;
            await localStore.RemoveItemAsync("currentUserEmail");
            await localStore.RemoveItemAsync("isKoordinator");
            // skal måske også rydde andre relaterede data? spørg ole
        }




        // Andre nødvendige metoder
    }
}
