using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using VagtplanApp.Server.Repositories;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Controllers
{
    [ApiController]
    [Route("api/persons")]

    public class PersonController : ControllerBase
    {
        private IPersonRepository mRepo;

        // Konstruktør til at injicere repository
        public PersonController(IPersonRepository repo)
        {
            mRepo = repo;
        }

        // GetAll
        [HttpGet]
        [Route("getall")]
        public List<Person> GetAll()
        {
            return mRepo.GetAll();
        }

        // Add Person
        [HttpPost]
        [Route("add")]
        public async Task CreatePerson([FromBody] Person person) // FromBody indikerer at data for booking forventes at blive sendt som en JSON
        {
            await mRepo.AddPerson(person);
        }

        // Get Email --> til login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Person loginPerson)
        {
            var person = await mRepo.GetPersonByEmail(loginPerson.Email);
            if (person != null && person.Password == loginPerson.Password)
            {
                // Succesfuld login
                return Ok(new { Email = person.Email, IsKoordinator = person.isKoordinator });
            }
            else
            {
                // Login mislykkedes
                return Unauthorized();
            }
        }

    }
}
