using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using VagtplanApp.Server.Repositories;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Controllers
{
    [ApiController]
    [Route("api/vagter")]

    public class VagterController : ControllerBase
    {
        private IVagterRepository mRepo;

        // Konstruktør til at injicere repository
        public VagterController(IVagterRepository repo)
        {
            mRepo = repo;
        }

        // GetAll vagter
        [HttpGet]
        [Route("getall")]
        public List<Vagter> GetAll()
        {
            return mRepo.GetAll();
        }

        // Opretter en person
        [HttpPost]
        [Route("add")]
        public async Task CreateVagter([FromBody] Vagter vagter) 
        {
            await mRepo.AddVagter(vagter);
        }

        // Frivillige kan tage vagter (SKAL VI TILFØJE IACTIONRESULT?)
        [HttpPut]
        [Route("takeshift/{vagtId}")]
        public async Task TakeShift(string vagtId, [FromBody] string personId)
        {
            await mRepo.TakeShift(vagtId, personId);
        }

        // Viser frivilliges vagter
        [HttpGet]
        [Route("person/{personId}")]
        public async Task<ActionResult<List<Vagter>>> GetShiftsByPersonId(string personId)
        {
                var vagter = await mRepo.GetShiftsByPersonId(personId);
                if (vagter == null || vagter.Count == 0)
                {
                    return NotFound();
                }

                return vagter;
        }
    }
}
    



