using AutoMapper;
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
    public class TreatmentTimeController : BaseApiController
    {
        private readonly TreatmentManager _treatmentManager;
        private readonly IMapper _mapper;

        public TreatmentTimeController(TreatmentManager treatmentManager, IMapper mapper)
        {
            _treatmentManager = treatmentManager;
            _mapper = mapper;
        }

        [HttpGet("treatment/{id}")]
        public async Task<IEnumerable<TreatmentTimeDto>> GetTreatmentTimesByTreatmentId(int id)
        {
            return _mapper.Map<IEnumerable<TreatmentTimeDto>>(await _treatmentManager.GetTreatmentTimesByTreatmentIdAsync(id)); 
        }

        [HttpGet("{id}")]
        public async Task<TreatmentTimeDto> GetTreatmentTimeById(int id)
        {
            return _mapper.Map<TreatmentTimeDto>(await _treatmentManager.GetTreatmentTimeByIdAsync(id));
        }

        [HttpPost]
        public async Task<TreatmentTimeDto> AddTreatmentTime(AddTreatmentTimeDto time)
        {
            return _mapper.Map<TreatmentTimeDto>(await _treatmentManager.AddTreatmentTime(time));
        }

        [HttpPut]
        public async Task<TreatmentTimeDto> UpdateTreatmentTime(UpdateTreatmentTimeDto time)
        {
            return _mapper.Map<TreatmentTimeDto>(await _treatmentManager.UpdateTreatmentTime(time));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTreatmentTime(int id)
        {
            if (await _treatmentManager.DeleteTreatmentTime(id))
                return Ok();
            return BadRequest();
        }

        [HttpPut("state/{id}")]
        public async Task<ActionResult> ChangeStateOfTreatmentTime(int id)
        {
            await _treatmentManager.ChageStateOfTreatmentTime(id);
            return Ok();
        }


    }
}
