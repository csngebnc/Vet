using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;

namespace Vet.Data.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly VetDbContext _context;

        public MedicalRecordRepository(VetDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddMedicalRecord(MedicalRecord record)
        {
            _context.MedicalRecords.Add(record);
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<bool> AddTherapiaToMedicalRecord(TherapiaRecord therapia)
        {
            _context.TherapiaRecords.Add(therapia);
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<bool> DeleteMedicalRecord(MedicalRecord record)
        {
            _context.Remove(record);
            return (await _context.SaveChangesAsync() > 0);
        }
        public async Task<bool> AddPhoto(MedicalRecordPhoto photoRecord)
        {
            _context.MedicalRecordPhotos.Add(photoRecord);
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<bool> RemovePhoto(MedicalRecordPhoto photoRecord)
        {
            _context.MedicalRecordPhotos.Remove(photoRecord);
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<MedicalRecord> GetMedicalRecordById(int id)
        {
            return await _context.MedicalRecords
                .Include("Doctor")
                .Include("Owner")
                .Include("Animal")
                .Include("Animal.Species")
                .Include("Photos")
                .Include(t => t.TherapiaRecords)
                    .ThenInclude(tr => tr.Therapia)
                .Where(m => m.Id == id).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecords()
        {
            return await _context.MedicalRecords
                .Include("Doctor")
                .Include("Owner")
                .Include("Animal")
                .Include("Animal.Species")
                .Include("Photos")
                .Include(t => t.TherapiaRecords)
                    .ThenInclude(tr => tr.Therapia)
                .OrderByDescending(r => r.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByAnimalId(int id)
        {
            return await _context.MedicalRecords
                .Include("Doctor")
                .Include("Owner")
                .Include("Animal")
                .Include("Animal.Species")
                .Include("Photos")
                .Include(t => t.TherapiaRecords)
                    .ThenInclude(tr => tr.Therapia)
                .OrderByDescending(r => r.Id)
                .Where(m => m.AnimalId == id).ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByDoctorId(string id)
        {
            return await _context.MedicalRecords
                .Include("Doctor")
                .Include("Owner")
                .Include("Animal")
                .Include("Animal.Species")
                .Include("Photos")
                .Include(t => t.TherapiaRecords)
                    .ThenInclude(tr => tr.Therapia)
                .OrderByDescending(r => r.Id)
                .Where(m => m.DoctorId == id).ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByUserEmail(string email)
        {
            return await _context.MedicalRecords
                .Include("Doctor")
                .Include("Owner")
                .Include("Animal")
                .Include("Animal.Species")
                .Include("Photos")
                .Include(t => t.TherapiaRecords)
                    .ThenInclude(tr => tr.Therapia)
                .OrderByDescending(r => r.Id)
                .Where(m => m.OwnerEmail == email).ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByUserId(string id)
        {
            return await _context.MedicalRecords
                .Include("Doctor")
                .Include("Owner")
                .Include("Animal")
                .Include("Animal.Species")
                .Include("Photos")
                .Include(t => t.TherapiaRecords)
                    .ThenInclude(tr => tr.Therapia)
                .OrderByDescending(r => r.Id)
                .Where(m => m.OwnerId == id).ToListAsync();
        }

        public async Task<bool> RemoveTherapiaFromMedicalRecord(TherapiaRecord therapia)
        {
            _context.TherapiaRecords.Remove(therapia);
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<MedicalRecord> UpdateMedicalRecord(MedicalRecord record)
        {
            _context.MedicalRecords.Update(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<TherapiaRecord> UpdateTherapiaOnMedicalRecord(TherapiaRecord therapiaRecord)
        {
            _context.TherapiaRecords.Update(therapiaRecord);
            await _context.SaveChangesAsync();
            return therapiaRecord;
        }

        public async Task<IEnumerable<TherapiaRecord>> GetTherapiaRecordsByRecordId(int id)
        {
            return await _context.TherapiaRecords.Include("Therapia").Where(m => m.MedicalRecordId == id).ToListAsync();
        }

        public async Task<TherapiaRecord> GetTherapiaRecordById(int id)
        {
            return await _context.TherapiaRecords.Include("Therapia").Where(m => m.Id == id).SingleOrDefaultAsync();
        }

        public async Task<MedicalRecordPhoto> GetMedicalRecordPhotoById(int photoId)
            => await _context.MedicalRecordPhotos.FindAsync(photoId);
    }
}
