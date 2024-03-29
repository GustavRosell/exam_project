﻿using VagtplanApp.Shared.Model;

namespace VagtplanApp.Server.Repositories
{
    public interface IShiftRepository
    {
        List<Shift> GetAllShifts();
        Task CreateShift(Shift vagter);
        Task AddPersonToShift(string vagtId, string personId);
        Task<List<Shift>> GetShiftsByPersonId(string personId);
        Task RemovePersonFromShift(string shiftId, string personId);
        Task UpdateShift(Shift updatedShift);
        Task DeleteShift(string shiftId);
    }
}
