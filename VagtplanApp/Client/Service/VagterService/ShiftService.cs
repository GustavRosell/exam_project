using System.Net.Http.Json;
using Blazored.LocalStorage;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Client.Services
{
    // ShiftService: Håndterer operationer relateret til vagter
    public class ShiftService : IShiftService
    {        
        private readonly HttpClient httpClient; // HTTP klient bruges til at lave web requests
        private readonly ILocalStorageService localStorage; // LocalStorage bruges til at gemme og hente data lokalt

        // Constructor: Initialiserer httpClient og localStorage
        public ShiftService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------

        // Henter alle vagter fra serveren
        public async Task<List<Shift>> GetAllShifts()
        {
            var shifts = await httpClient.GetFromJsonAsync<List<Shift>>("api/shift/getall");

            return shifts;
        }

        // Sortere alle shifts på prioritet
        public List<Shift> GetSortedShifts(List<Shift> shifts, bool sortByPriority)
        {
            return sortByPriority
                ? shifts.OrderByDescending(shift => shift.priority).ToList()
                : shifts.ToList();
        }

        // Opretter en ny vagt ved at sende data til serveren
        public async Task CreateShift(Shift shift)
        {
            await httpClient.PostAsJsonAsync("api/shift/add", shift);
        }

        // Tillader frivillige at tage en specifik vagt
        public async Task<bool> TakeShift(string shiftId)
        {
            // Henter den nuværende bruger fra LocalStorage
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");

            // Sender en anmodning til serveren for at tage en vagt
            var response = await httpClient.PutAsJsonAsync($"/api/shift/takeshift/{shiftId}", currentUser.id);

            // Returnerer resultatet af anmodningen
            return response.IsSuccessStatusCode;
        }

        // Henter vagter tildelt til den nuværende bruger
        public async Task<List<Shift>> GetShiftsForVolunteer()
        {
            try
            {
                var currentUser = await localStorage.GetItemAsync<Person>("currentUser");
                if (currentUser == null) return new List<Shift>();

                var shifts = await httpClient.GetFromJsonAsync<List<Shift>>($"api/shift/person/{currentUser.id}");
                return shifts ?? new List<Shift>();
            }
            catch (Exception ex)
            {
                // Log fejl eller håndter den på anden måde
                return new List<Shift>();
            }
        }

        // Fjerner den nuværende bruger fra en vagt
        public async Task RemovePersonFromShift(string shiftId)
        {
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");

            // Sender en anmodning til serveren for at fjerne den nuværende bruger fra en vagt
            await httpClient.PutAsync($"api/shift/removeperson/{shiftId}/{currentUser.id}", null);

        }

        // Opdaterer detaljer om en vagt
        public async Task UpdateShift(Shift updatedShift)
        {
            var response = await httpClient.PutAsJsonAsync("api/shift/updateshift", updatedShift);
        }

        // Kontroller om tidsrummet for den ønskede vagt overlapper med nogen af brugerens eksisterende vagter
        private bool ShiftsOverlap(Shift attemptToTakeShift, Shift userShift)
        {

            return attemptToTakeShift.startTime < userShift.endTime && attemptToTakeShift.endTime > userShift.startTime;
        }

        // "forsåger" at tage en vagt --> Håndterer forskellige scenarier som overlapning
        public async Task<string> TryTakeShift(string shiftId)
        {
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");

            var allShifts = await GetAllShifts();
            var attemptToTakeShift = allShifts.FirstOrDefault(s => s.id == shiftId);
            if (attemptToTakeShift.assignedPersons.Count >= attemptToTakeShift.numberOfPersons)
                return "FullyBooked";

            var userShifts = await GetShiftsForVolunteer();
            foreach (var userShift in userShifts)
            {
                if (ShiftsOverlap(attemptToTakeShift, userShift))
                    return "TimeOverlap";
            }

            var takeSuccess = await TakeShift(shiftId);
            return takeSuccess ? "Success" : "Error";
        }

        // Sletter en vagt.
        public async Task DeleteShift(string shiftId)
        {
            await httpClient.DeleteAsync($"api/shift/deleteshift/{shiftId}");
        }
    }
}
