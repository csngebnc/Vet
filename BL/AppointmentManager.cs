using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;

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
            var _appointment = new Appointment
            {
                StartDate = appointment.StartDate.ToLocalTime(),
                EndDate = appointment.EndDate.ToLocalTime(),
                OwnerId = userId,
                AnimalId = appointment.AnimalId,
                DoctorId = appointment.DoctorId,
                TreatmentId = appointment.TreatmentId
            };

            return await _appointmentRepository.AddAppointment(_appointment);


        }
        public async Task<AppointmentDto> ResignAppointment(int id) => _mapper.Map<Appointment, AppointmentDto>(await _appointmentRepository.ResignAppointment(id));

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
    }
}
