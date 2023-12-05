using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Client.Services
{
    public class ShiftService : IShiftService
    {
        private readonly HttpClient httpClient; // HTTP klient bruges til at lave web requests
        private readonly ILocalStorageService localStorage;


        public ShiftService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;

        }
        public async Task<List<Shift>> GetAllShifts()
        {
            var shifts = await httpClient.GetFromJsonAsync<List<Shift>>("api/shift/getall");

            return shifts;
        }

        public async Task CreateShift(Shift shift)
        {
            // Send vagt til serveren for at blive gemt
            await httpClient.PostAsJsonAsync("api/shift/add", shift);
        }

        // Frivillige kan tage vagter
        public async Task<bool> TakeShift(string shiftId)
        {
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");
            if (currentUser != null)
            {
                var response = await httpClient.PutAsJsonAsync($"/api/shift/takeshift/{shiftId}", currentUser.id);
                return response.IsSuccessStatusCode;
            }
            return false;
        }

        // Metode for at hente vagter for en bestemt frivillig
        public async Task<List<Shift>> GetShiftsForVolunteer()
        {
            // Henter den nuværende bruger fra LocalStorage
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");

            // Tjekker om der er en nuværende bruger
            if (currentUser != null)
            {
                // Sender en GET-anmodning til serveren for at hente vagter for den nuværende bruger
                var shitfs = await httpClient.GetFromJsonAsync<List<Shift>>($"api/shift/person/{currentUser.id}");

                // Hvis der ikke er nogen vagter for brugeren, returnerer en tom liste
                if (shitfs == null || shitfs.Count == 0)
                {
                    return new List<Shift>();
                }

                return shitfs;
            }

            // Hvis der ikke er nogen nuværende bruger, returneres en tom liste
            return new List<Shift>();
        }
    }
}
