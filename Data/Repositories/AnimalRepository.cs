using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public class AnimalRepository : IAnimalRepository
    {
        private readonly VetDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoManager _photoManager;

        public AnimalRepository(VetDbContext context, IMapper mapper, IPhotoManager photoManager)
        {
            _context = context;
            _mapper = mapper;
            _photoManager = photoManager;
        }

        public async Task<Animal> AddAnimal(AddAnimalDto animal, string OwnerId)
        {
            var _animal = _mapper.Map<Animal>(animal);
            _animal.OwnerId = OwnerId;

            var photoPath = await _photoManager.UploadAnimalPhoto(animal.Photo, OwnerId);
            _animal.PhotoPath = photoPath == null ? "Images/Animals/empty-photo.jpg" : photoPath;

            await _context.Animals.AddAsync(_animal);
            await _context.SaveChangesAsync();

            return _animal;
        }

        public async Task<AnimalDto> UpdateAnimal(UpdateAnimalDto animal)
        {
            var _animal = await _context.Animals
                .Include("Owner")
                .Where(a => a.Id == animal.Id)
                .FirstOrDefaultAsync();

            _mapper.Map<UpdateAnimalDto, Animal>(animal, _animal);
            await _context.SaveChangesAsync();
            return _mapper.Map<AnimalDto>(_animal);
        }

        public async Task<AnimalDto> UpdateAnimalPhoto(UpdateAnimalPhotoDto animal, string OwnerId)
        {
            var _animal = await _context.Animals.FindAsync(animal.Id);
            var photoPath = await _photoManager.UploadAnimalPhoto(animal.Photo, OwnerId);
            _animal.PhotoPath = photoPath;

            await _context.SaveChangesAsync();

            return _mapper.Map<AnimalDto>(_animal);
        }

        public async Task DeleteAnimalPhoto(int id)
        {
            var _animal = await _context.Animals.FindAsync(id);
            if (!_animal.PhotoPath.Equals("Images/Animals/empty-photo.jpg"))
            {
                _photoManager.RemovePhoto(_animal.PhotoPath);
                _animal.PhotoPath = "Images/Animals/empty-photo.jpg";
            }
            await _context.SaveChangesAsync();

        }

        public async Task<bool> DeleteAnimal(int id)
        {
            var _animal = await _context.Animals.FindAsync(id);
            // TODO: bővíteni

            if(_animal.PhotoPath != "Images/Animals/empty-photo.jpg")
                _photoManager.RemovePhoto(_animal.PhotoPath);

            _context.Animals.Remove(_animal);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task ArchiveAnimalById(int id)
        {
            // TODO: később, amikor jobban világossá válik, hogy miként szeretném
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AnimalDto>> GetAnimalsByUserIdAsync(string id)
        {
            return _mapper.Map<IEnumerable<AnimalDto>>(
                await _context.Animals
                    .Include("Owner")
                    .Where(a => a.OwnerId == id)
                    .ToListAsync());
        }

        public async Task<AnimalDto> GetAnimalByIdAsync(int id)
        {
            return _mapper.Map<AnimalDto>(
                await _context.Animals
                    .Include("Owner")
                    .Where(a => a.Id == id)
                    .FirstOrDefaultAsync());
        }

        public async Task<IEnumerable<AnimalDto>> GetAnimalsAsync()
        {
            return _mapper.Map<IEnumerable<AnimalDto>>(
                await _context.Animals
                    .Include("Owner")
                    .ToListAsync());
        }




        public async Task<bool> AnimalExists(int id)
        {
            return await _context.Animals.AnyAsync(a => a.Id == id);
        }



    }
}
