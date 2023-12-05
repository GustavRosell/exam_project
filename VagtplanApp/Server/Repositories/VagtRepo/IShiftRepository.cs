using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories
{
    public interface IShiftRepository
    {
        List<Shift> GetAllShifts();
        Task CreateShift(Shift vagter);
        Task TakeShift(string vagtId, string personId);
        Task<List<Shift>> GetShiftsByPersonId(string personId);
    }
}
