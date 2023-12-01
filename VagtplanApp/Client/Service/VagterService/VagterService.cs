using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using VagtplanApp.Shared.Model;
using static System.Net.WebRequestMethods;

namespace VagtplanApp.Client.Services
{
    public class VagterService : IVagterService
    {
        private readonly HttpClient httpClient; // HTTP klient bruges til at lave web requests

        public VagterService(HttpClient httpClient)
        {
            this.httpClient = httpClient;        
        }
        public async Task<List<Vagter>> GetAllVagter()
        {
            var vagter = await httpClient.GetFromJsonAsync<List<Vagter>>("api/vagter/getall");

            return vagter;
        }

        public async Task CreateShift(Vagter vagter)
        {
            await httpClient.PostAsJsonAsync<Vagter>("api/vagter/add", vagter);
        }
    }
}
