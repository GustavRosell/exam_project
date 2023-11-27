using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using VagtplanApp.Server.Repositories;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class PersonController : ControllerBase
    {
        private IPersonRepository mRepo;


        // GetAll
        [HttpGet]
        public List<Person> GetAll()
        {
            return mRepo.GetAll();
        }

        // Add Person
        [HttpPost]
        public async Task CreatePerson([FromBody] Person person) // FromBody indikerer at data for booking forventes at blive sendt som en JSON
        {
            await mRepo.AddPerson(person);
        }

    }
}