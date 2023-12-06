using System.Threading.Tasks;
using VagtplanApp.Shared.Model;

namespace VagtplanApp.Client.Services
{
    public interface IShiftService
    {
        Task<List<Shift>> GetAllShifts();
        Task CreateShift(Shift shift);
        Task<bool> TakeShift(string shiftId);
        Task<List<Shift>> GetShiftsForVolunteer();
        Task RemovePersonFromShift(string shiftId);
    }
}
