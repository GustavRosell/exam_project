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

        // GetAll
        [HttpGet]
        [Route("getall")]
        public List<Vagter> GetAll()
        {
            return mRepo.GetAll();
        }

        // Add Person
        [HttpPost]
        [Route("add")]
        public async Task CreateVagter([FromBody] Vagter vagter) // FromBody indikerer at data for booking forventes at blive sendt som en JSON
        {
            await mRepo.AddVagter(vagter);
        }

        [HttpPut]
        [Route("updateshift/{vagtId}")]
        public async Task<IActionResult> UpdateVagt(string vagtId, [FromBody] List<string> assignedPersonIds)
        {
            await mRepo.UpdateShift(vagtId, assignedPersonIds);
            return Ok();
        }

    }


}
