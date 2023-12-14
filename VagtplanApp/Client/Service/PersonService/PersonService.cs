using System.Net.Http.Json;
using Blazored.LocalStorage;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Client.Services
{
    // PersonService: Håndterer brugerrelaterede operationer.
    public class PersonService : IPersonService
    {
        public Person CurrentUser { get; private set; } // Holder styr på den nuværende bruger
        private readonly HttpClient httpClient; // HTTP klient bruges til at lave web requests // interagere med browserens lokale lagring
        private readonly ILocalStorageService localStore; // Til at gemme og hente brugerdata fra lokal storage

        // Constructor: Initialiserer httpClient og LocalStorage
        public PersonService(HttpClient httpClient, ILocalStorageService localStore)
        {
            this.httpClient = httpClient;
            this.localStore = localStore;
        }

        // Tilføjer en ny bruger via API anmodning
        public async Task<bool> AddPerson(Person person) // Task<bool>??
        {
            var response = await httpClient.PostAsJsonAsync("/api/persons/add", person);
            return response.IsSuccessStatusCode;
        }

        // Sætter den nuværende bruger i applikationen
        public void SetCurrentUser(Person user)
        {
            CurrentUser = user;
        }

        // Tjekker, om den nuværende bruger er en koordinator
        public bool IsKoordinator()
        {
            return CurrentUser != null && CurrentUser.isKoordinator;
        }

        // Autentificerer en bruger baseret på email og password
        public async Task<Person> Authenticate(string email, string password)
        {
            var loginPerson = new Person { email = email, password = password };
            var response = await httpClient.PostAsJsonAsync("api/persons/authenticate", loginPerson);

            // Kontrollerer om serverens svar var en succes (HTTP statuskode 200).
            if (response.IsSuccessStatusCode)
            {
                CurrentUser = await response.Content.ReadFromJsonAsync<Person>();

                // Gemmer den nuværende bruger i local storage for at opretholde brugerens tilstand på tværs af sessioner.
                await localStore.SetItemAsync("currentUser", CurrentUser);

                return CurrentUser;
            }
            else
            {
                // Hvis serverens respons ikke var en succes, returneres null for at indikere, at autentificeringen mislykkedes.
                return null;
            }
        }

        // Tjekker, om en bruger er logget ind
        public async Task<bool> IsUserLoggedInAsync()
        {
            // Tjekker om brugeren er logget ind ved først at se på CurrentUser og derefter i localStorage
            if (CurrentUser == null)
            {
                CurrentUser = await localStore.GetItemAsync<Person>("currentUser");
            }
            return CurrentUser != null;
        }

        // Logger brugeren ud og fjerner session fra local storage
        public async Task LogOut()
        {
            // Sætter nuværende bruger til null
            CurrentUser = null;

            // Rydder brugerdata fra local storage
            await localStore.RemoveItemAsync("currentUser");
        }

        // Opdaterer en brugers oplysninger
        public async Task UpdatePerson(Person updatedPerson)
        {
            // Logger en besked til konsollen med ID for den person, der bliver opdateret.
            Console.WriteLine($"Sending update for person with ID: {updatedPerson.id}");
            var response = await httpClient.PutAsJsonAsync("api/persons/updateperson", updatedPerson);

            // Logger en fejlmeddelelse til konsollen med HTTP-statuskoden for fejlen.
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to update person: {response.StatusCode}");
            }
        }
    }
}
