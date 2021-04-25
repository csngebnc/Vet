using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vet.BL;
using Vet.BL.Exceptions;
using Vet.Extensions;
using Vet.Models.DTOs;

namespace Vet.Controllers
{
    [Authorize]
    public class AnimalController : BaseApiController
    {
        private readonly AnimalManager _animalManager;

        public AnimalController(AnimalManager animalManager)
        {
            _animalManager = animalManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAllAnimals()
            => Ok(await _animalManager.GetAnimalsAsync());

        [HttpGet("my-animals")]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetMyAnimals()
            => Ok(await _animalManager.GetAnimalsByUserIdAsync(User.GetById()));
        
        [HttpGet("my-archived-animals")]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetArchivedAnimalsByUserId()
            => Ok(await _animalManager.GetArchivedAnimalsByUserIdAsync(User.GetById()));

        [HttpGet("get/{id}")]
        public async Task<ActionResult<AnimalDto>> GetAnimalById(int id)
            => await _animalManager.GetAnimalByIdAsync(id);

        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAnimalsByUserEmail(string email)
            => Ok(await _animalManager.GetAnimalsByUserEmailAsync(email));

        [HttpGet("by-id/{id}")]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAnimalsByUserId(string id)
            => Ok(await _animalManager.GetAnimalsByUserIdAsync(id));

        [HttpGet("archived-by-id/{id}")]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetArchivedAnimalsByUserId(string id)
            => Ok(await _animalManager.GetArchivedAnimalsByUserIdAsync(id));
        
        [HttpPost("addAnimal")]
        public async Task<ActionResult<AnimalDto>> AddAnimal([FromForm]AddAnimalDto animal)
            => Ok(await _animalManager.AddAnimal(animal));

        [HttpPut("updateAnimal")]
        public async Task<ActionResult<AnimalDto>> UpdateAnimal(UpdateAnimalDto animal)
            => Ok(await _animalManager.UpdateAnimal(animal));

        [HttpPut("archiveAnimal/{id}")]
        public async Task<ActionResult> UpdateAnimal(int id)
        {
            await _animalManager.ChangeStateOfAnimal(id);
            return Ok();
        }

        [HttpPut("updatePhoto")]
        public async Task<ActionResult> UpdateAnimalPhoto([FromForm]UpdateAnimalPhotoDto animal)
        {
            if (!(await _animalManager.AnimalExists(animal.Id)))
                return NotFound();

            await _animalManager.UpdateAnimalPhoto(animal);
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
            => (await _animalManager.DeleteAnimal(id)) ? Ok() : BadRequest();
        
    }
}
