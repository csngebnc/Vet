using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;

namespace Vet.Data.Repositories
{
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly VetDbContext _context;

        public TreatmentRepository(VetDbContext context)
        {
            _context = context;
        }

        public async Task<Treatment> AddTreatment(Treatment treatment)
        {
            _context.Treatments.Add(treatment);
            await _context.SaveChangesAsync();
            return treatment;
        }

        public async Task<Treatment> UpdateTreatment(Treatment treatment)
        {
            var t = await _context.Treatments.FindAsync(treatment.Id);
            t.Name = treatment.Name;
            await _context.SaveChangesAsync();
            return await this.GetTreatmentByIdAsync(treatment.Id);
        }

        public async Task<bool> DeleteTreatment(int id)
        {
            if ((await _context.TreatmentTimes.AnyAsync(tt => tt.TreatmentId == id)) || (await _context.Appointments.AnyAsync(a => a.TreatmentId == id)))
                return false;

            _context.Treatments.Remove(await this.GetTreatmentByIdAsync(id));
            return (await _context.SaveChangesAsync()) > 0;

        }


        public async Task<IEnumerable<Treatment>> GetTreatmentsAsync()
            => await _context.Treatments.Include("Doctor").ToListAsync();

        public async Task<Treatment> GetTreatmentByIdAsync(int id)
            => await _context.Treatments.Include("Doctor").Where(t => t.Id == id).SingleOrDefaultAsync();
        public async Task<IEnumerable<Treatment>> GetTreatmentsByDoctorIdAsync(string id)
            => await _context.Treatments.Include("Doctor").Where(t => t.DoctorId == id).ToListAsync();


        ///////////////////////////////////////////////////////////////////////


        public async Task<TreatmentTime> AddTreatmentTime(TreatmentTime time)
        {
            _context.TreatmentTimes.Add(time);
            await _context.SaveChangesAsync();
            return time;
        }

        public async Task<TreatmentTime> UpdateTreatmentTime(TreatmentTime time)
        {
            var _time = await this.GetTreatmentTimeByIdAsync(time.Id);
            _time.StartHour = time.StartHour;
            _time.StartMin = time.StartMin;
            _time.EndHour = time.EndHour;
            _time.EndMin= time.EndMin;
            _time.Duration = time.Duration;
            _time.DayOfWeek = time.DayOfWeek;
            await _context.SaveChangesAsync();
            return _time;
        }

        public async Task<bool> DeleteTreatmentTime(TreatmentTime time)
        {
            _context.TreatmentTimes.Remove(time);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<TreatmentTime> GetTreatmentTimeByIdAsync(int id)
            => await _context.TreatmentTimes.FindAsync(id);

        public async Task<IEnumerable<TreatmentTime>> GetTreatmentTimesByTreatmentIdAsync(int id)
            => await _context.TreatmentTimes.Where(t => t.TreatmentId == id).ToListAsync();
    }
}
