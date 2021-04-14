using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;
using Vet.Models.DTOs.Appointment;

namespace Vet.BL
{
    public class AppointmentManager
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        public AppointmentManager(IMapper mapper, IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddAppointment(AddAppointmentByUserDto appointment, string userId) 
        {
            var _appointment = _mapper.Map<Appointment>(appointment);
            _appointment.OwnerId = userId;

            return await _appointmentRepository.AddAppointment(_appointment);
        }

        public async Task<bool> AddAppointmentByDoctor(AddAppointmentByDoctorDto appointment)
            => await _appointmentRepository.AddAppointment(_mapper.Map<Appointment>(appointment));

        public async Task<AppointmentDto> ResignAppointment(int id, string details = "Lemondva") 
            => _mapper.Map<Appointment, AppointmentDto>(await _appointmentRepository.ResignAppointment(id, details));

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointments()
            => _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetAllAppointments());
        public async Task<IEnumerable<AppointmentDto>> GetActiveAppointments()
            => _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetActiveAppointments());
        public async Task<IEnumerable<AppointmentDto>> GetInactiveAppointments()
            => _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetInactiveAppointments());
        public async Task<IEnumerable<AppointmentTimeDto>> GetReservedTimes(DateTime time, string doctorId)
            => _mapper.Map<IEnumerable<AppointmentTimeDto>>(await _appointmentRepository.GetReservedTimes(time, doctorId));

        public async Task<IEnumerable<AppointmentDto>> GetUserAppointments(string id)
            => _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetUserAppointments(id));

        public async Task<IEnumerable<AppointmentDto>> GetDoctorActiveAppointments(string id)
            => _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetDoctorActiveAppointments(id));
        public async Task<IEnumerable<AppointmentDto>> GetDoctorInactiveAppointments(string id)
            => _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetDoctorInactiveAppointments(id));
    }
}
