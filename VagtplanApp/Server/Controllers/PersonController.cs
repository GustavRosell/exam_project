using Microsoft.AspNetCore.Mvc;
using VagtplanApp.Server.Repositories;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Controllers
{
    [ApiController]
    [Route("api/persons")]

    // PersonController: Håndterer HTTP-anmodninger relaterert til brugere
    public class PersonController : ControllerBase
    {
        // Repository injiceret via konstruktør
        private IPersonRepository mRepo; 

        public PersonController(IPersonRepository repo)
        {
            mRepo = repo;
        }

        // Henter alle personer
        [HttpGet]
        [Route("getall")]
        public List<Person> GetAll()
        {
            return mRepo.GetAllPersons();
        }

        // Tilføjer en ny person 
        [HttpPost]
        [Route("add")]
        public async Task CreatePerson([FromBody] Person person) // FromBody indikerer at data for booking forventes at blive sendt som en JSON
        {
            await mRepo.CreatePerson(person);
        }

        // Autentificerer en bruger
        [HttpPost]
        [Route("authenticate")]
        public async Task<ActionResult<Person>> Authenticate([FromBody] Person loginPerson)
        {
            //Først tjekker vi om input-email passer med email i MongoDB
            var person = await mRepo.GetPersonByEmail(loginPerson.email);
            // Hvis email passer, tjekker vi nu om personens password matcher med input-password. 
            if (person != null && person.password == loginPerson.password)
            {
                return person;
            }
            else
            {
                return Unauthorized(); // Returnerer HTTP 401 hvis login er ugyldigt
            }
        }

        // Opdaterer en persons oplysninger
        [HttpPut]
        [Route("updateperson")]
        public async Task<IActionResult> UpdatePerson([FromBody] Person person)
        {
            await mRepo.UpdatePerson(person);
            Console.WriteLine($"Received update for person: {person.email}");
            return Ok();
        }
    }
}
