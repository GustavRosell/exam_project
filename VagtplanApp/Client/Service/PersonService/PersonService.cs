using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Client.Services
{
    public class PersonService : IPersonService
    {
        private readonly HttpClient httpClient; // HTTP klient bruges til at lave web requests
        private readonly ILocalStorageService localStore; // Til at gemme og hente brugerdata fra lokal storage
        //private readonly NavigationManager navigationManager; // Til Log-ud så vi kan navigere tilbage til forside

        public Person CurrentUser { get; private set; } // Holder styr på den nuværende bruger

        public PersonService(HttpClient httpClient, ILocalStorageService localStore)
        {
            this.httpClient = httpClient;
            this.localStore = localStore;
            //this.navigationManager = navigationManager;
        }

        public async Task<bool> AddPerson(Person person) // Task<bool>??
        {
            //var latestPersonResponse = await httpClient.GetAsync("/api/persons/getlatest");
            //var latestPerson = await latestPersonResponse.Content.ReadFromJsonAsync<Person>();
            //int maxPersonalId = latestPerson?.personalId ?? 0;
            //person.personalId = maxPersonalId + 1;

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
            var loginPerson = new Person { email = email, password = password };
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

        public async Task LogOut()
        {
            // Sætter nuværende bruger til null
            CurrentUser = null;

            // Rydder brugerdata fra local storage
            await localStore.RemoveItemAsync("currentUser");

            // Navigerer til forsiden
            //navigationManager.NavigateTo("/");
        }
    }
}
