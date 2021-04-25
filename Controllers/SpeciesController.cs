using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL;
using Vet.Models.DTOs;

namespace Vet.Controllers
{ 
    public class SpeciesController : BaseApiController
    {
        private readonly SpeciesManager _speciesManager;

        public SpeciesController(SpeciesManager speciesManager)
        {
            _speciesManager = speciesManager;
        }

        [HttpGet]
        public async Task<IEnumerable<AnimalSpeciesDto>> GetAnimalSpecies()
        {
            return await _speciesManager.GetAnimalSpecies();
        }

        [HttpGet("{id}")]
        public async Task<AnimalSpeciesDto> GetAnimalSpeciesById(int id)
        {
            return await _speciesManager.GetAnimalSpeciesById(id);
        }

        [HttpPost]
        public async Task<ActionResult<AnimalSpeciesDto>> AddAnimalSpecies(AddSpeciesDto spec)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _speciesManager.AddAnimalSpecies(spec.Name));
        }

        [HttpPut]
        public async Task<ActionResult<AnimalSpeciesDto>> UpdateAnimalSpecies(UpdateAnimalSpeciesDto spec)
        {
            return await _speciesManager.UpdateAnimalSpecies(spec);
        }

        [HttpPut("state/{id}")]
        public async Task<ActionResult> ChangeStateOfAnimalSpecies(int id)
        {
            await _speciesManager.ChageStateOfSpecies(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnimalSpecies(int id)
        {
            if (await _speciesManager.DeleteAnimalSpecies(id))
                return Ok();

            return BadRequest();
        }
    }
}
