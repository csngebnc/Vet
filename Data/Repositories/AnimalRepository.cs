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
    public class AnimalRepository : IAnimalRepository
    {
        private readonly VetDbContext _context;

        public AnimalRepository(VetDbContext context)
        {
            _context = context;
        }

        public async Task<Animal> AddAnimal(Animal animal)
        {
            await _context.Animals.AddAsync(animal);
            await _context.SaveChangesAsync();

            return animal;
        }

        public async Task<Animal> UpdateAnimal(Animal animal)
        {
            _context.Animals.Update(animal);
            await _context.SaveChangesAsync();
            return await this.GetAnimalByIdAsync(animal.Id);
        }

        public async Task<bool> DeleteAnimal(Animal animal)
        {
            if(await _context.Appointments.AnyAsync(a => a.AnimalId == animal.Id) ||
                await _context.MedicalRecords.AnyAsync(m => m.AnimalId == animal.Id) ||
                await _context.VaccinationRecords.AnyAsync(vr => vr.AnimalId == animal.Id))
                return false;

            _context.Animals.Remove(animal);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task ChangeStateOfAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            animal.IsArchived = !animal.IsArchived;
            await _context.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<Animal>> GetAnimalsAsync()
        {
            return await _context.Animals
                    .Include("Owner")
                    .Include("Species")
                    .ToListAsync();
        }
        public async Task<IEnumerable<Animal>> GetAnimalsByUserIdAsync(string id)
        {
            return await _context.Animals
                    .Include("Owner")
                    .Include("Species")
                    .Where(a => a.OwnerId == id && !a.IsArchived)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Animal>> GetAnimalsByUserEmailAsync(string email)
        {
            return await _context.Animals
                    .Include("Owner")
                    .Include("Species")
                    .Where(a => a.Owner.Email == email && !a.IsArchived)
                    .ToListAsync();
        }

        public async Task<Animal> GetAnimalByIdAsync(int id)
        {
            return await _context.Animals
                    .Include("Owner")
                    .Include("Species")
                    .Where(a => a.Id == id)
                    .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Animal>> GetArchivedAnimalsByUserId(string id)
        {
            return await _context.Animals
                    .Include("Owner")
                    .Include("Species")
                    .Where(a => a.OwnerId == id && a.IsArchived)
                    .ToListAsync();
        }


    }
}
