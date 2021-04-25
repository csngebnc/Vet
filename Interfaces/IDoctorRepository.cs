using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Models;

namespace Vet.Interfaces
{
    public interface IDoctorRepository
    {
        Task<VetUser> PromoteToDoctor(VetUser user);
        Task<VetUser> DemoteToUser(VetUser doctor);

        Task<VetUser> GetDoctorById(string id);
        Task<VetUser> GetDoctorByEmail(string email);

        Task<IEnumerable<VetUser>> GetDoctors();

        Task<Holiday> AddHoliday(Holiday holiday);
        Task<Holiday> EditHoliday(Holiday holiday);
        Task<bool> DeleteHoliday(Holiday holiday);

        Task<Holiday> GetHolidayById(int id);
        Task<IEnumerable<Holiday>> GetDoctorsHolidays(string doctorId);
        Task<IEnumerable<Holiday>> GetHolidays();
        Task<bool> HolidayExists(int holidayId);

    }
}
