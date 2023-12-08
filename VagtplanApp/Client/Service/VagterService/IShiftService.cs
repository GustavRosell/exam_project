using VagtplanApp.Shared.Model;
using static VagtplanApp.Client.Services.ShiftService;

public interface IShiftService
{
    Task<List<Shift>> GetAllShifts();
    Task CreateShift(Shift shift);
    Task<bool> TakeShift(string shiftId);
    Task<List<Shift>> GetShiftsForVolunteer();
    Task RemovePersonFromShift(string shiftId);
    Task UpdateShift(Shift updatedShift);
    List<Shift> GetSortedShifts(List<Shift> shifts, bool sortByPriority);
    Task<string> TryTakeShift(string shiftId);
    Task DeleteShift(string shiftId);
}
