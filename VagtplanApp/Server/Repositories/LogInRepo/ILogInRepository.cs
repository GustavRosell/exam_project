using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories
{
    public interface ILogInRepository
    {
        List<LogIn> GetAll();

    }
}