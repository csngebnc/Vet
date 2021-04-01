using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;

namespace Vet.Data.Repositories
{
    public class VaccineRepository : IVaccineRepository
    {
        private readonly VetDbContext _context;

        public VaccineRepository(VetDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddVaccine(Vaccine vaccine)
        {
            _context.Vaccines.Add(vaccine);
            return (await _context.SaveChangesAsync()) > 0;
        }
        public async Task<Vaccine> UpdateVaccine(Vaccine vaccine)
        {
            _context.Vaccines.Update(vaccine);
            await _context.SaveChangesAsync();
            return vaccine;
        }
        public async Task<bool> DeleteVaccine(Vaccine vaccine)
        {
            if ((await _context.VaccinationRecords.AnyAsync(vr => vr.VaccineId == vaccine.Id)))
                return false;

            _context.Vaccines.Remove(vaccine);
            return (await _context.SaveChangesAsync()) > 0;

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<bool> AddVaccineRecord(VaccineRecord record)
        {
            _context.VaccinationRecords.Add(record);
            return (await _context.SaveChangesAsync()) > 0;
        }
        public async Task<VaccineRecord> UpdateVaccineRecord(VaccineRecord record)
        {
            await _context.SaveChangesAsync();
            return record;
        }
        public async Task<bool> DeleteVaccineRecord(VaccineRecord record)
        {
            _context.VaccinationRecords.Remove(record);
            return (await _context.SaveChangesAsync()) > 0;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<IEnumerable<Vaccine>> GetVaccines()
            => await _context.Vaccines.ToListAsync();

        public async Task<Vaccine> GetVaccineById(int id)
            => await _context.Vaccines.Where(v => v.Id ==id).SingleOrDefaultAsync();

        /////////////////////////////////////////////////////////////////////////////////////

        public async Task<IEnumerable<VaccineRecord>> GetVaccineRecordsOfAnimal(int animalId)
            => await _context.VaccinationRecords.Include("Vaccine").Include("Animal").Where(vr => vr.AnimalId == animalId).ToListAsync();

        public async Task<VaccineRecord> GetVaccineRecordById(int id)
            => await _context.VaccinationRecords.Include("Vaccine").Include("Animal").Where(v => v.Id == id).SingleOrDefaultAsync();


    }
}
