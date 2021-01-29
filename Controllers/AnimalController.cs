using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vet.BL;
using Vet.Extensions;
using Vet.Models.DTOs;

namespace Vet.Controllers
{
    public class AnimalController : BaseApiController
    {
        private readonly AnimalManager _animalManager;

        public AnimalController(AnimalManager animalManager)
        {
            _animalManager = animalManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAllAnimals()
        {
            return Ok(await _animalManager.GetAnimalsAsync());
        }

        [HttpGet("my-animals")]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetMyAnimals()
        {
            return Ok(await _animalManager.GetAnimalsByUserIdAsync(User.GetById()));
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<AnimalDto>> GetAnimalById(int id)
        {
            if(await _animalManager.AnimalExists(id))
                return await _animalManager.GetAnimalByIdAsync(id);
            return NotFound();
        }

        [HttpPost("addAnimal")]
        public async Task<ActionResult> AddAnimal([FromForm]AddAnimalDto animal)
        {
            await _animalManager.AddAnimal(animal, User.GetById());
            return Ok();
        }

        [HttpPut("updateAnimal")]
        public async Task<ActionResult> UpdateAnimal(UpdateAnimalDto animal)
        {
            if (!(await _animalManager.AnimalExists(animal.Id)))
                return NotFound();

            await _animalManager.UpdateAnimal(animal);
            return Ok();
        }

        [HttpPut("updatePhoto")]
        public async Task<ActionResult> UpdateAnimalPhoto([FromForm]UpdateAnimalPhotoDto animal)
        {
            if (!(await _animalManager.AnimalExists(animal.Id)))
                return NotFound();

            await _animalManager.UpdateAnimalPhoto(animal, User.GetById());
            return Ok();
        }

        [HttpDelete("deletePhoto/{animal_id}")]
        public async Task<ActionResult> DeleteAnimalPhoto(int animal_id)
        {
            if (!(await _animalManager.AnimalExists(animal_id)))
                return NotFound();
            await _animalManager.DeleteAnimalPhoto(animal_id);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnimal(int id)
        {
            if(await _animalManager.DeleteAnimal(id))
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
