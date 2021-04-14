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
    public class DoctorRepository : IDoctorRepository
    {
        private readonly VetDbContext _context;

        public DoctorRepository(VetDbContext context)
        {
            _context = context;
        }

        public async Task<VetUser> PromoteToDoctor(VetUser user)
        {
            user.AuthLevel = 2;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<VetUser> DemoteToUser(VetUser doctor)
        {
            doctor.AuthLevel = 1;
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<VetUser> GetDoctorById(string id)
            => await _context.Users.Where(d => d.Id == id && d.AuthLevel >= 2).SingleOrDefaultAsync();


        public async Task<VetUser> GetDoctorByEmail(string email)
            => await _context.Users.Where(d => d.Email == email).SingleOrDefaultAsync();

        public async Task<IEnumerable<VetUser>> GetDoctors()
            => await _context.Users.Where(u => u.AuthLevel >= 2).ToListAsync();

        public async Task<Holiday> AddHoliday(Holiday holiday)
        {
            _context.Holidays.Add(holiday);
            await _context.SaveChangesAsync();
            return holiday;

        }

        public async Task<Holiday> EditHoliday(Holiday holiday)
        {
            var _holiday = await this.GetHolidayById(holiday.Id);
            _holiday.StartDate = holiday.StartDate;
            _holiday.EndDate = holiday.EndDate;
            await _context.SaveChangesAsync();
            return _holiday;
        }

        public async Task<bool> DeleteHoliday(Holiday holiday)
        {
            _context.Remove(holiday);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Holiday> GetHolidayById(int id)
            => await _context.Holidays.Include("Doctor").Where(d => d.Id == id).SingleOrDefaultAsync();

        public async Task<IEnumerable<Holiday>> GetDoctorsHolidays(string doctorId)
            => await _context.Holidays.Include("Doctor").Where(d => d.DoctorId == doctorId).ToListAsync();

        public async Task<IEnumerable<Holiday>> GetHolidays()
            => await _context.Holidays.Include("Doctor").ToListAsync();
    }
}
