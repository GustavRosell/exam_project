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

        [HttpPost]
        [Route("authenticate")]
        public async Task<ActionResult<Person>> Authenticate([FromBody] Person loginPerson)
        {

            var person = await mRepo.GetPersonByEmail(loginPerson.Email);
            if (person != null && person.Password == loginPerson.Password)
            {
                return person;
            }
            else
            {
                return Unauthorized(); // Returnerer HTTP 401 hvis login er ugyldigt
            }
        }

    }


}
