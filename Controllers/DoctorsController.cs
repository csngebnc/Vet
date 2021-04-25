using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL;
using Vet.Data;
using Vet.Extensions;
using Vet.Interfaces;
using Vet.Models.DTOs;

namespace Vet.Controllers
{
    public class DoctorsController : BaseApiController
    {
        private readonly DoctorManager _doctorManager;
        private readonly IMapper _mapper;

        public DoctorsController(DoctorManager doctorManager, IMapper mapper)
        {
            _doctorManager = doctorManager;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IEnumerable<VetUserDto>> GetDoctors()
        {
            return await _doctorManager.GetDoctors();
        }

        [HttpPost("promote/{email}")]
        public async Task<VetUserDto> PromoteUser(string email)
        {
            return await _doctorManager.PromoteToDoctor(email);
        }

        [HttpPost("demote/{id}")]
        public async Task<VetUserDto> DemoteDoctor(string id)
        {
            return await _doctorManager.DemoteToUser(id);
        }

        [HttpPost("holiday/add")]
        public async Task<HolidayDto> AddHoliday(AddHolidayDto holiday)
        {
            return await _doctorManager.AddHoliday(holiday);
        }

        [HttpPut("holiday/update")]
        public async Task<HolidayDto> UpdateHoliday(HolidayDto holiday)
        {
            return await _doctorManager.EditHoliday(holiday);
        }

        [HttpDelete("holiday/delete/{id}")]
        public async Task<bool> DeleteHoliday(int id)
        {
            return await _doctorManager.DeleteHoliday(id);
        }

        [HttpGet("holiday/get/{id}")]
        public async Task<HolidayDto> GetHolidayById(int id)
            => await _doctorManager.GetHolidayById(id);

        [HttpGet("{id}/holiday")]
        public async Task<IEnumerable<HolidayDto>> GetDoctorHolidays(string id)
            => await _doctorManager.GetDoctorsHolidays(id);

        [HttpGet("holiday")]
        public async Task<IEnumerable<HolidayDto>> GetLoggedInDoctorHolidays()
            => await _doctorManager.GetDoctorsHolidays(User.GetById());

        [HttpGet("holiday/all")]
        public async Task<IEnumerable<HolidayDto>> GetHolidays()
            => await _doctorManager.GetHolidays();
    }
}
