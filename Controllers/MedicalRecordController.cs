﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL;
using Vet.Extensions;
using Vet.Models.DTOs;
using Vet.Models.DTOs.MedicalRecord;

namespace Vet.Controllers
{
    [Route("api/records")]
    [ApiController]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MedicalRecordManager _medicalRecordManager;
        public MedicalRecordController(MedicalRecordManager medicalRecordManager, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _medicalRecordManager = medicalRecordManager;
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddMedicalRecord(AddMedicalRecordDto record)
        {
            var id = await _medicalRecordManager.AddMedicalRecord(record);
            return Ok(id);
        }

        [HttpPut]
        public async Task<MedicalRecordDto> UpdateMedicalRecord(UpdateMedicalRecordDto record)
        {
            return await _medicalRecordManager.UpdateMedicalRecord(record);
        }

        [HttpDelete("therapia/{id}")]
        public async Task<bool> RemoveTherapiaFromMedicalRecord(int id)
        {
            return await _medicalRecordManager.RemoveTherapiaFromMedicalRecord(id);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteMedicalRecord(int id)
        {
            return await _medicalRecordManager.DeleteMedicalRecord(id);
        }

        [HttpPost("add-photo/{medId}")]
        public async Task<bool> AddPhoto(IFormFile file, int medId)
        {
              return await _medicalRecordManager.UploadPhoto(file, medId);
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<bool> DeletePhoto( int photoId)
        {
            return await _medicalRecordManager.DeletePhoto(photoId);
        }

        [HttpGet("id/{id}")]
        public async Task<MedicalRecordDto> GetMedicalRecordById(int id)
        {
            return await _medicalRecordManager.GetMedicalRecordById(id);
        }

        [HttpGet("animalid/{id}")]
        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByAnimalId(int id)
        {
            return await _medicalRecordManager.GetMedicalRecordsByAnimalId(id);
        }

        [HttpGet("my-records")]
        public async Task<IEnumerable<MedicalRecordDto>> GetMyMedicalRecords()
        {
            return await _medicalRecordManager.GetCurrentUserMedicalRecords();
        }

        [HttpGet("userid/{id}")]
        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByUserId(string id)
        {
            return await _medicalRecordManager.GetMedicalRecordsByUserId(id);
        }

        [HttpGet("pdf/{id}")]
        public async Task<IActionResult> GetPdf(int id)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "pdfs");
            var content = new MemoryStream(await _medicalRecordManager.GeneratePdf(id, path));
            var contentType = "APPLICATION/octet-stream";
            return File(content, contentType);
        }
    }
}
