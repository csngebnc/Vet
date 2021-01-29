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
        private readonly IMapper _mapper;

        public AnimalRepository(VetDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            // TODO: bővíteni, hogy ha van rá hivatkozás, akkor már nem lehet törölni
            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task ArchiveAnimalById(int id)
        {
            // TODO: később, amikor jobban világossá válik, hogy miként szeretném
            throw new NotImplementedException();
        }

        
        public async Task<bool> AddAnimalSpecies(string name)
        {
            //if (await SpeciesExists(name)) return false;
            _context.AnimalSpecies.Add(new AnimalSpecies { Name = name });
            return (await _context.SaveChangesAsync()) > 0;

        }
        public async Task<AnimalSpecies> UpdateAnimalSpecies(AnimalSpecies spec)
        {
            _context.AnimalSpecies.Update(spec);
            await _context.SaveChangesAsync();
            return await this.GetAnimalSpeciesById(spec.Id);
        }
        public async Task<bool> DeleteAnimalSpecies(AnimalSpecies spec)
        {
            _context.AnimalSpecies.Remove(spec);
            return (await _context.SaveChangesAsync() > 0);
        }

        
        public async Task<IEnumerable<Animal>> GetAnimalsAsync()
        {
            return await _context.Animals
                    .Include("Owner")
                    .ToListAsync();
        }
        public async Task<IEnumerable<Animal>> GetAnimalsByUserIdAsync(string id)
        {
            return await _context.Animals
                    .Include("Owner")
                    .Where(a => a.OwnerId == id)
                    .ToListAsync();
        }

        public async Task<Animal> GetAnimalByIdAsync(int id)
        {
            return await _context.Animals
                    .Include("Owner")
                    .Where(a => a.Id == id)
                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AnimalSpecies>> GetAnimalSpecies()
        {
            return await _context.AnimalSpecies.ToListAsync();
        }

        public async Task<AnimalSpecies> GetAnimalSpeciesById(int id)
        {
            return await _context.AnimalSpecies.FindAsync(id);
        }
        public async Task<AnimalSpecies> GetAnimalSpeciesByName(string name)
        {
            return await _context.AnimalSpecies.Where(s => s.Name == name).FirstOrDefaultAsync();
        }

   



    }
}
