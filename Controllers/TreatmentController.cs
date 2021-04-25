using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL;
using Vet.Extensions;
using Vet.Models.DTOs;

namespace Vet.Controllers
{
    public class TreatmentController : BaseApiController
    {
        private readonly TreatmentManager _treatmentManager;

        public TreatmentController(TreatmentManager treatmentManager)
        {
            _treatmentManager = treatmentManager;
        }

        [HttpGet]
        public async Task<IEnumerable<TreatmentDto>> GetAllTreatment()
        {
            return await _treatmentManager.GetAllTreatments();
        }

        [HttpGet("treatments/{id}")]
        public async Task<TreatmentDto> GetTreatmentById(int id)
        {
            return await _treatmentManager.GetTreatmentByIdAsync(id);
        }

        [HttpGet("doctor/{id}")]
        public async Task<IEnumerable<TreatmentDto>> GetTreatmentsByDoctorId(string id)
        {
            return await _treatmentManager.GetTreatmentsByDoctorId(id);
        }

        [HttpGet("my-treatments")]
        public async Task<IEnumerable<TreatmentDto>> GetMyTreatments()
        {
            return await _treatmentManager.GetMyTreatments(User.GetById());
        }

        [HttpPost]
        public async Task<TreatmentDto> AddTreatment(AddTreatmentDto addTreatmentDto)
        {
            return await _treatmentManager.AddTreatment(addTreatmentDto);
        }

        [HttpPut]
        public async Task<ActionResult<TreatmentDto>> UpdateTreatment(UpdateTreatmentDto treatment)
        {
            return await _treatmentManager.UpdateTreatment(treatment);
        }

        [HttpPut("state/{id}")]
        public async Task<ActionResult> ChangeStateOfTreatment(int id)
        {
            await _treatmentManager.ChageStateOfTreatment(id);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteAnimalSpecies(int id)
        {
            if (await _treatmentManager.DeleteTreatment(id))
                return Ok();

            return BadRequest();
        }




        //[HttpGet("times/{id}")]

        //[HttpGet("time/{id}")]

    }
}
