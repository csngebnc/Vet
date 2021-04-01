using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.Controllers
{
    public class TherapiaController : BaseApiController
    {
        private readonly TherapiaManager _therapiaManager;

        public TherapiaController(TherapiaManager therapiaManager)
        {
            _therapiaManager = therapiaManager;
        }

        [HttpGet]
        public async Task<IEnumerable<Therapia>> GetTherapias()
        {
            return await _therapiaManager.GetTherapias();
        }

        [HttpGet("{id}")]
        public async Task<Therapia> GetAnimalSpeciesById(int id)
        {
            return await _therapiaManager.GetTherapiaById(id);
        }

        [HttpPost]
        public async Task<ActionResult> AddAnimalSpecies(AddTherapiaDto therapia)
        {
            if (await _therapiaManager.AddTherapia(therapia))
                return Ok();

            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult<Therapia>> UpdateTherapia(Therapia therapia)
        {
            return await _therapiaManager.UpdateTherapia(therapia);
        }

        [HttpPut("state/{id}")]
        public async Task<ActionResult> ChangeStateOfTherapia(int id)
        {
            await _therapiaManager.ChageStateOfTherapia(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTherapia(int id)
        {
            if (await _therapiaManager.DeleteTherapia(id))
                return Ok();

            return BadRequest();
        }
    }
}
