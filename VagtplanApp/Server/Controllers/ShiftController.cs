using Microsoft.AspNetCore.Mvc;
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

        // Henter alle vagter
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

        // Frivillige kan tage vagter
        [HttpPut]
        [Route("takeshift/{shiftId}")]
        public async Task AddPersonToShift(string shiftId, [FromBody] string personId)
        {
            await mRepo.AddPersonToShift(shiftId, personId);
        }

        // Viser frivilliges vagter
        [HttpGet]
        [Route("showvoshifts/{personId}")]
        public async Task<ActionResult<List<Shift>>> GetShiftsByPersonId(string personId)
        {
            var shifts = await mRepo.GetShiftsByPersonId(personId);
            if (shifts == null)
            {
                return new List<Shift>();
            }

            return shifts;
        }

        // Fjerner en person fra en vagt
        [HttpPut]
        [Route("removeperson/{shiftId}/{personId}")]
        public async Task<IActionResult> RemovePersonFromShift(string shiftId, string personId)
        {
            await mRepo.RemovePersonFromShift(shiftId, personId);
            return Ok();
        }

        // Opdatere data for en vagt
        [HttpPut]
        [Route("updateshift")]
        public async Task<IActionResult> UpdatePerson([FromBody] Shift shift)
        {
            await mRepo.UpdateShift(shift);
            return Ok();
        }

        // Sletter en vagt
        [HttpDelete]
        [Route("deleteshift/{shiftId}")]
        public async Task DeleteShift(string shiftId)
        {
            await mRepo.DeleteShift(shiftId);

        }
    }
}
