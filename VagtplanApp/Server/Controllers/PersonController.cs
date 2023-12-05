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
            return mRepo.GetAllPersons();
        }

        // Add Person
        [HttpPost]
        [Route("add")]
        public async Task CreatePerson([FromBody] Person person) // FromBody indikerer at data for booking forventes at blive sendt som en JSON
        {
            await mRepo.CreatePerson(person);
        }

        //Authenticate routen
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

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdatePerson([FromBody] Person updatePerson)
        {
            try
            {
                // Retrieve the existing person from the database
                var existingPerson = await mRepo.GetPersonByEmail(updatePerson.email);

                if (existingPerson == null)
                {
                    return NotFound();
                }

                // Update the properties of the existing person
                existingPerson.firstName = updatePerson.firstName;
                existingPerson.lastName = updatePerson.lastName;
                existingPerson.email = updatePerson.email;
                existingPerson.password = updatePerson.password;
                // Update other properties as needed

                // Save the changes to the database
                await mRepo.UpdatePerson(existingPerson);

                return Ok(existingPerson);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}
