using System.Threading.Tasks;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Client.Services
{
    public interface IVagterService
    {
        Task<List<Vagter>> GetAllVagter();
        Task CreateShift(Vagter vagter);

        Task<bool> UpdateVagt(string vagtId);
    }
}
