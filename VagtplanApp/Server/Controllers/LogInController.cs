using Microsoft.AspNetCore.Mvc;
using VagtplanApp.Server.Repositories.LogInRepo;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Controllers.LogInController
{
    [ApiController]
    [Route("api/LogIn")]
    public class LogInController : ControllerBase
    {
        private ILogInRepository mRepo;

        public LogInController(ILogInRepository repo)
        {
            this.mRepo = repo;
        }

        [HttpGet]
        [Route("getall")]
        public List<LogIn> GetAll()
        {
            return mRepo.GetAll();
        }
    }
}
