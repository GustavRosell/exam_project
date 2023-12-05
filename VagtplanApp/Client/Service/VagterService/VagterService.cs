using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Client.Services
{
    public class VagterService : IVagterService
    {
        private readonly HttpClient httpClient; // HTTP klient bruges til at lave web requests
        private readonly ILocalStorageService localStorage;


        public VagterService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;

        }
        public async Task<List<Vagter>> GetAllVagter()
        {
            var vagter = await httpClient.GetFromJsonAsync<List<Vagter>>("api/vagter/getall");

            return vagter;
        }

        public async Task CreateShift(Vagter vagt)
        {
            // Send vagt til serveren for at blive gemt
            await httpClient.PostAsJsonAsync("api/vagter/add", vagt);
        }

        // Frivillige kan tage vagter
        public async Task<bool> TakeShift(string vagtId)
        {
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");
            if (currentUser != null)
            {
                var response = await httpClient.PutAsJsonAsync($"/api/vagter/takeshift/{vagtId}", currentUser.id);
                return response.IsSuccessStatusCode;
            }
            return false;
        }

        // Metode for at hente vagter for en bestemt frivillig
        public async Task<List<Vagter>> GetShiftsForVolunteer()
        {
            // Henter den nuværende bruger fra LocalStorage
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");

            // Tjekker om der er en nuværende bruger
            if (currentUser != null)
            {
                // Sender en GET-anmodning til serveren for at hente vagter for den nuværende bruger
                var vagter = await httpClient.GetFromJsonAsync<List<Vagter>>($"api/vagter/person/{currentUser.id}");

                // Hvis der ikke er nogen vagter for brugeren, returnerer en tom liste
                if (vagter == null || vagter.Count == 0)
                {
                    return new List<Vagter>();
                }

                return vagter;
            }

            // Hvis der ikke er nogen nuværende bruger, returneres en tom liste
            return new List<Vagter>();
        }
    }
}
