using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Data;
using Vet.Extensions;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.Controllers
{
    public class AnimalController : BaseApiController
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly VetDbContext _db;

        public AnimalController(IAnimalRepository animalRepository, VetDbContext db)
        {
            _animalRepository = animalRepository;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAllAnimals()
        {
            return Ok(await _animalRepository.GetAnimalsAsync());
        }

        [HttpGet("my-animals")]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetMyAnimals()
        {
            return Ok(await _animalRepository.GetAnimalsByUserIdAsync(User.GetById()));
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<AnimalDto>> GetAnimalById(int id)
        {
            if(await _animalRepository.AnimalExists(id))
                return await _animalRepository.GetAnimalByIdAsync(id);
            return NotFound();
        }

        [HttpPost("addAnimal")]
        public async Task<ActionResult> AddAnimal([FromForm]AddAnimalDto animal)
        {
            await _animalRepository.AddAnimal(animal, User.GetById());
            return Ok();
        }

        [HttpPut("updateAnimal")]
        public async Task<ActionResult> UpdateAnimal(UpdateAnimalDto animal)
        {
            if (!(await _animalRepository.AnimalExists(animal.Id)))
                return NotFound();

            await _animalRepository.UpdateAnimal(animal);
            return Ok();
        }

        [HttpPut("updatePhoto")]
        public async Task<ActionResult> UpdateAnimalPhoto([FromForm]UpdateAnimalPhotoDto animal)
        {
            if (!(await _animalRepository.AnimalExists(animal.Id)))
                return NotFound();

            await _animalRepository.UpdateAnimalPhoto(animal, User.GetById());
            return Ok();
        }

        [HttpDelete("deletePhoto/{animal_id}")]
        public async Task<ActionResult> DeleteAnimalPhoto(int animal_id)
        {
            if (!(await _animalRepository.AnimalExists(animal_id)))
                return NotFound();
            await _animalRepository.DeleteAnimalPhoto(animal_id);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnimal(int id)
        {
            if(await _animalRepository.DeleteAnimal(id))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        
    }
}
