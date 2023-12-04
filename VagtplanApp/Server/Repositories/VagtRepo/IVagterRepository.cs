using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories
{
    public interface IVagterRepository
    {
        List<Vagter> GetAll();
        Task AddVagter(Vagter vagter);
        Task UpdateShift(string vagtId, string personId);
    }
}
