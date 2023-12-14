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

        // Henter alle vagter fra serveren
        public async Task<List<Shift>> GetAllShifts()
        {
            var shifts = await httpClient.GetFromJsonAsync<List<Shift>>("api/shift/getall");

            return shifts;
        }

        // Sorterer alle shifts baseret på prioritet, hvis sortByPriority er true
        public List<Shift> GetSortedShifts(List<Shift> shifts, bool sortByPriority)
        {
            if (sortByPriority)
            {
                shifts.Sort((shift1, shift2) => shift2.priority.CompareTo(shift1.priority));
            }

            return shifts;
        }

        // Sorterer alle shifts på antal tildelte
        public List<Shift> GetShiftsSortedByAssignment(List<Shift> shifts)
        {
            // Sorterer listen således, at vagten som mangler flest frivillige kommer først på oversigten
            return shifts.OrderByDescending(shift => shift.numberOfPersons - shift.assignedPersons.Count).ToList();
        }

        // Opretter en ny vagt
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

        // Asynkron metode til at hente en liste over vagter tildelt den nuværende frivillige bruger.
        public async Task<List<Shift>> GetShiftsForVolunteer()
        {
            // Henter den nuværende bruger fra local storage
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");

            // Henter vagter for nuværende bruger
            var shifts = await httpClient.GetFromJsonAsync<List<Shift>>($"api/shift/showvoshifts/{currentUser.id}");

            if (shifts == null)
            {
                // Hvis ingen vagter tildelt, laver tom liste
                return new List<Shift>();
            }
            // Viser vagt liste
            return shifts;
        }

        // Fjerner den nuværende bruger fra en vagt
        public async Task RemovePersonFromShift(string shiftId)
        {
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");

            // Sender en DELETE anmodning til serveren for at fjerne den nuværende bruger fra en vagt
            await httpClient.DeleteAsync($"api/shift/removeperson/{shiftId}/{currentUser.id}");
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

        // "forsøger" at tage en vagt --> Håndterer forskellige scenarier som overlapning og fuld vagt
        public async Task<string> TryTakeShift(string shiftId)
        {
            // Henter den nuværende bruger fra lokal lagring.
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");

            // Henter en liste over alle vagter.
            var allShifts = await GetAllShifts();

            // Finder den specifikke vagt, brugeren forsøger at tage, baseret på det givne ID.
            var attemptToTakeShift = allShifts.FirstOrDefault(s => s.id == shiftId);

            // Kontrollerer, om vagten allerede har nået det maksimale antal tildelte personer.
            if (attemptToTakeShift.assignedPersons.Count >= attemptToTakeShift.numberOfPersons)
                return "FullyBooked"; // Returnerer en indikator for, at vagten er fuldt booket.

            // Henter en liste over vagter, som den nuværende bruger allerede er tildelt.
            var userShifts = await GetShiftsForVolunteer();

            // Tjekker for tidsmæssige overlap mellem den ønskede vagt og brugerens eksisterende vagter.
            foreach (var userShift in userShifts)
            {
                // Hvis der findes et overlap, returneres en indikator for dette.
                if (ShiftsOverlap(attemptToTakeShift, userShift))
                    return "TimeOverlap";
            }

            // Forsøger at tage den ønskede vagt.
            var takeSuccess = await TakeShift(shiftId);

            // Returnerer en succesindikator, hvis vagten blev taget, ellers returnerer en fejlindikator.
            return takeSuccess ? "Success" : "Error"; 
        }

        // Sletter en vagt.
        public async Task DeleteShift(string shiftId)
        {
            await httpClient.DeleteAsync($"api/shift/deleteshift/{shiftId}");
        }
    }
}
