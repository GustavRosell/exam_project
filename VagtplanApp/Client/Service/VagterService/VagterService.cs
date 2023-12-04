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
        public async Task<bool> UpdateVagt(string vagtId)
        {
            var currentUser = await localStorage.GetItemAsync<Person>("currentUser");
            if (currentUser != null)
            {
                var response = await httpClient.PutAsJsonAsync($"/api/vagter/updateshift/{vagtId}", currentUser.id);
                return response.IsSuccessStatusCode;
            }
            return false;
        }

    }
}
