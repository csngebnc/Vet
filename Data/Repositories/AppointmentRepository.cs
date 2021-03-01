using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.Data.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly VetDbContext _context;
        private readonly IMapper _mapper;

        public AppointmentRepository(VetDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            return (await _context.SaveChangesAsync()) > 0;
        }
        public async Task<Appointment> ResignAppointment(int id)
        {
            var appointment = await this.GetAppointmentById(id);
            appointment.IsResigned = true;
            appointment.Details = "Lemondva";
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointments()
            => await _context.Appointments.Include("Treatment").Include("Doctor").Include("Owner").Include("Animal").ToListAsync();
        public async Task<IEnumerable<Appointment>> GetActiveAppointments()
            => await _context.Appointments.Include("Treatment").Include("Doctor").Include("Owner").Include("Animal").Where(a => !a.IsResigned).ToListAsync();

        public async Task<IEnumerable<Appointment>> GetInactiveAppointments()
            => await _context.Appointments.Include("Treatment").Include("Doctor").Include("Owner").Include("Animal").Where(a => a.IsResigned).ToListAsync();

        public async Task<IEnumerable<Appointment>> GetReservedTimes(DateTime time, string doctorId)
            => await _context.Appointments.Where(a => a.DoctorId == doctorId && a.StartDate.DayOfYear == time.DayOfYear && !a.IsResigned).ToListAsync();

        public async Task<IEnumerable<Appointment>> GetUserAppointments(string id)
            => await _context.Appointments.Include("Treatment").Include("Doctor").Include("Owner").Include("Animal").Where(a => a.OwnerId == id).OrderByDescending(a => a.StartDate).ThenByDescending(a => a.Id).ToListAsync();

        public async Task<Appointment> GetAppointmentById(int id) 
            => await _context.Appointments.Include("Treatment").Include("Doctor").Include("Owner").Include("Animal").Where(a => a.Id == id).SingleOrDefaultAsync();

    }
}
