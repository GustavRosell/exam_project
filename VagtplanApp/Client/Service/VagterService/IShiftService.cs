using VagtplanApp.Shared.Model;

public interface IShiftService
{
    Task<List<Shift>> GetAllShifts();
    Task CreateShift(Shift shift);
    Task<bool> TakeShift(string shiftId);
    Task<List<Shift>> GetShiftsForVolunteer();
    Task RemovePersonFromShift(string shiftId);
    Task UpdateShift(Shift updatedShift);
    List<Shift> GetSortedShifts(List<Shift> shifts, bool sortByPriority);
    Task<bool> TryTakeShift(string shiftId);
    Task DeleteShift(string shiftId);
}
