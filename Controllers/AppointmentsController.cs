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
using Vet.Models;
using Vet.Models.DTOs;
using Vet.Models.DTOs.Appointment;

namespace Vet.Controllers
{
    public class AppointmentsController : BaseApiController
    {
        private readonly VetDbContext _context;
        private readonly IMapper _mapper;
        private readonly AppointmentManager _appointmentManager;

        public AppointmentsController(AppointmentManager appointmentManager, VetDbContext context, IMapper mapper)
        {
            _appointmentManager = appointmentManager;
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IEnumerable<VetUserDto>> GetDoctors()
        {
            return _mapper.Map<IEnumerable<VetUserDto>>(await _context.Users.Where(u => u.AuthLevel == 1).ToListAsync());
        }

        [HttpPost]
        public async Task<bool> BookAppointment(AddAppointmentByUserDto appointment)
        {
            return await _appointmentManager.AddAppointment(appointment, User.GetById());
        }

        [HttpPost("by-doctor")]
        public async Task<bool> BookAppointmentByDoctor(AddAppointmentByDoctorDto appointment)
        {
            return await _appointmentManager.AddAppointmentByDoctor(appointment);
        }


        [HttpGet("getreserved")]
        public async Task<IEnumerable<AppointmentTimeDto>> GetReservedTimes([FromQuery] DateTime time, [FromQuery] string doctorId)
        {
            return await _appointmentManager.GetReservedTimes(time, doctorId);
        }

        [HttpPut("resign/{id}")]
        public async Task<AppointmentDto> ResignByUser(int id)
        {
            return await _appointmentManager.ResignAppointment(id);
        }

        [HttpPut("resign-by-doctor/{id}")]
        public async Task<AppointmentDto> ResignByDoctor(int id)
        {
            return await _appointmentManager.ResignAppointment(id, "Lemondva orvos által");
        }

        [HttpGet("my-appointments")]
        public async Task<IEnumerable<AppointmentDto>> GetMyAppointments()
        {
            return await _appointmentManager.GetUserAppointments(User.GetById());
        }


        [HttpGet("doctor-active-appointments/{id}")]
        public async Task<IEnumerable<AppointmentDto>> GetDoctorActiveAppointments(string id)
        {
            return await _appointmentManager.GetDoctorActiveAppointments(id);
        }

        [HttpGet("doctor-inactive-appointments/{id}")]
        public async Task<IEnumerable<AppointmentDto>> GetDoctorInactiveAppointments(string id)
        {
            return await _appointmentManager.GetDoctorInactiveAppointments(id);
        }
    }
}
