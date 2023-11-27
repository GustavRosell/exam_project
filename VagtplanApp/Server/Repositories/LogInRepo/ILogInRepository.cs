using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories.LogInRepo
{
    public interface ILogInRepository
    {
        List<LogIn> GetAll();
    }
}