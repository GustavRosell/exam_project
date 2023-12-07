using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Client.Services
{
    public class ShiftService : IShiftService
    {
        // HTTP klient bruges til at lave web requests
        private readonly HttpClient httpClient; 
        // LocalStorage bruges til at gemme og hente data lokalt
        private readonly ILocalStorageService localStorage; 

        public ShiftService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        // Henter alle vagter fra serveren
        public async Task<List<Shift>> GetAllShifts()
        {
            var shifts = await httpClient.GetFromJsonAsync<List<Shift>>("api/shift/getall");

            return shifts;
        }

        // Opretter en ny vagt ved at sende data til serveren
        public async Task CreateShift(Shift shift)
        {
            await httpClient.PostAsJsonAsync("api/shift/add", shift);
        }

        // Frivillige kan tage vagter
        public async Task<bool> TakeShift(string shiftId)
        {
            // Henter den nuværende bruger fra LocalStorage
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");

            // Sender en anmodning til serveren for at tage en vagt
            var response = await httpClient.PutAsJsonAsync($"/api/shift/takeshift/{shiftId}", currentUser.id);

            // Returnerer resultatet af anmodningen
            return response.IsSuccessStatusCode;
        }


        // Henter vagter for den aktuelle bruger
        public async Task<List<Shift>> GetShiftsForVolunteer()
        {
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");

            // Sender en GET-anmodning til serveren for at hente vagter for den nuværende bruger
            var shifts = await httpClient.GetFromJsonAsync<List<Shift>>($"api/shift/person/{currentUser.id}");

            // Hvis der ikke er nogen vagter for brugeren, returnerer en tom liste
            if (shifts == null || shifts.Count == 0)
            {
                return new List<Shift>();
            }

            return shifts;
        }

        // Personer kan fjerne sig selv fra en vagt
        public async Task RemovePersonFromShift(string shiftId)
        {
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");

            // Sender en anmodning til serveren for at fjerne den nuværende bruger fra en vagt
            await httpClient.PutAsync($"api/shift/removeperson/{shiftId}/{currentUser.id}", null);
            
        }

        public async Task UpdateShift(Shift updatedShift)
        {
            var response = await httpClient.PutAsJsonAsync("api/shift/updateshift", updatedShift);
        }
    }
}
