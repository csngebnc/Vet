using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL;
using Vet.Models.DTOs.Vaccine;

namespace Vet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineController : BaseApiController
    {
        private readonly VaccineManager _vaccineManager;
        public VaccineController(VaccineManager vaccineManager)
        {
            _vaccineManager = vaccineManager;
        }

        [HttpGet("get")]
        public async Task<IEnumerable<VaccineDto>> GetVaccines()
            => await _vaccineManager.GetVaccines();

        [HttpGet("get/{id}")]
        public async Task<VaccineDto> GetVaccineById(int id)
            => await _vaccineManager.GetVaccineById(id);

        [HttpGet("record-by-animal/{animalId}")]
        public async Task<IEnumerable<VaccineRecordDto>> GetVaccineRecordsOfAnimal(int animalId)
            => await _vaccineManager.GetVaccineRecordsOfAnimal(animalId);

        [HttpGet("record-by-id/{id}")]
        public async Task<VaccineRecordDto> GetVaccineRecordById(int id)
            => await _vaccineManager.GetVaccineRecordById(id);

        [HttpPost]
        public async Task<VaccineDto> AddVaccine(AddVaccineDto vaccine)
            => await _vaccineManager.AddVaccine(vaccine);

        [HttpPut]
        public async Task<VaccineDto> UpdateVaccine(VaccineDto vaccine)
            => await _vaccineManager.UpdateVaccine(vaccine);

        [HttpDelete("{id}")]
        public async Task<bool> DeleteVaccine(int id)
            => await _vaccineManager.DeleteVaccine(id);

        [HttpPost("record")]
        public async Task<VaccineRecordDto> AddVaccineRecord(AddVaccineRecordDto record)
            => await _vaccineManager.AddVaccineRecord(record);

        [HttpPut("record")]
        public async Task<VaccineRecordDto> UpdateVaccineRekord(UpdateVaccineRecordDto record)
            => await _vaccineManager.UpdateVaccineRekord(record);

        [HttpDelete("record/{id}")]
        public async Task<bool> DeleteVaccineRecord(int id)
            => await _vaccineManager.DeleteVaccineRecord(id);
    }
}
