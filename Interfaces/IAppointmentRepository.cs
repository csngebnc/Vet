using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<bool> AddAppointment(Appointment appointment);
        Task<Appointment> ResignAppointment(int id, string details);
        Task<IEnumerable<Appointment>> GetAllAppointments();
        Task<IEnumerable<Appointment>> GetActiveAppointments();
        Task<IEnumerable<Appointment>> GetInactiveAppointments();
        Task<IEnumerable<Appointment>> GetReservedTimes(DateTime time, string doctorId);

        Task<IEnumerable<Appointment>> GetUserAppointments(string id);

        Task<Appointment> GetAppointmentById(int id);

        Task<IEnumerable<Appointment>> GetDoctorActiveAppointments(string id);
        Task<IEnumerable<Appointment>> GetDoctorInactiveAppointments(string id);
        Task<bool> AppointmentExists(int appointmentId);
    }
}
