using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using VagtplanApp.Server.Repositories;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Controllers
{
    [ApiController]
    [Route("api/shift")]

    public class ShiftController : ControllerBase
    {
        private IShiftRepository mRepo;

        // Konstruktør til at injicere repository
        public ShiftController(IShiftRepository repo)
        {
            mRepo = repo;
        }

        // GetAll vagter
        [HttpGet]
        [Route("getall")]
        public List<Shift> GetAll()
        {
            return mRepo.GetAllShifts();
        }

        // Opretter en person
        [HttpPost]
        [Route("add")]
        public async Task CreateShift([FromBody] Shift shift)
        {
            await mRepo.CreateShift(shift);
        }

        // Frivillige kan tage vagter (SKAL VI TILFØJE IACTIONRESULT?)
        [HttpPut]
        [Route("takeshift/{shiftId}")]
        public async Task TakeShift(string shiftId, [FromBody] string personId)
        {
            await mRepo.TakeShift(shiftId, personId);
        }

        // Viser frivilliges vagter
        [HttpGet]
        [Route("person/{personId}")] // SKAL ÆNDRES TIL AT VÆRE shift/shitfs
        public async Task<ActionResult<List<Shift>>> GetShiftsByPersonId(string personId)
        {
            var shifts = await mRepo.GetShiftsByPersonId(personId);
            if (shifts == null)
            {
                return new List<Shift>();
            }

            return shifts;
        }

        [HttpPut]
        [Route("removeperson/{shiftId}/{personId}")]
        public async Task<IActionResult> RemovePersonFromShift(string shiftId, string personId)
        {
            await mRepo.RemovePersonFromShift(shiftId, personId);
            return Ok();
        }

    }
}




