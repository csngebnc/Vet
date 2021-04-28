using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Vet.BL.Exceptions;
using Vet.Extensions;
using Vet.Helpers;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.BL
{
    public class AppointmentManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AppointmentManager(IHttpContextAccessor httpContextAccessor, IMapper mapper, IAppointmentRepository appointmentRepository, ITreatmentRepository treatmentRepository, IUserRepository userRepository, IAnimalRepository animalRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _appointmentRepository = appointmentRepository;
            _treatmentRepository = treatmentRepository;
            _userRepository = userRepository;
            _animalRepository = animalRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddAppointment(AddAppointmentDto appointment)
        {
            var error = new DataErrorException();

            var ownerId = appointment.OwnerId ?? _httpContextAccessor.GetCurrentUserId();

            await ValidateUser(error, appointment, ownerId);

            await ValidateAppointment(error, appointment, ownerId);

            var _appointment = _mapper.Map<Appointment>(appointment);
            _appointment.OwnerId = ownerId;

            return await _appointmentRepository.AddAppointment(_appointment);
        }

        public async Task<AppointmentDto> ResignAppointment(int id, string details = "Lemondva")
        {
            ValidationHelper.ValidateEntity(await _appointmentRepository.AppointmentExists(id), "foglalt időpont");
            var appointment = await _appointmentRepository.GetAppointmentById(id);

            var user = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(appointment.OwnerId == user.Id || user.AuthLevel > 1);

            return _mapper.Map<Appointment, AppointmentDto>(await _appointmentRepository.ResignAppointment(id, details));
        } 

        public async Task<IEnumerable<AppointmentTimeDto>> GetReservedTimes(DateTime time, string doctorId)
        {
            ValidationHelper.ValidateEntity(await _userRepository.UserExists(doctorId), "regisztrált doktor");
            var user = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(user.AuthLevel > 1);

            return _mapper.Map<IEnumerable<AppointmentTimeDto>>(await _appointmentRepository.GetReservedTimes(time, doctorId));
        }

        public async Task<IEnumerable<AppointmentDto>> GetUserAppointments(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateEntity(await _userRepository.UserExists(id), "felhasználó");
            ValidationHelper.ValidatePermission(_httpContextAccessor.GetCurrentUserId() == id || user.AuthLevel > 1);

            return _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetUserAppointments(id));
        }

        public async Task<IEnumerable<AppointmentDto>> GetDoctorActiveAppointments(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(user.AuthLevel > 1);

            ValidationHelper.ValidateEntity(await _userRepository.UserExists(id), "felhasználó");

            var doctor = await _userRepository.GetUserByIdAsync(id);
            ValidationHelper.ValidateEntity(doctor.AuthLevel > 1, "doktor");

            return _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetDoctorActiveAppointments(id));
        }
        public async Task<IEnumerable<AppointmentDto>> GetDoctorInactiveAppointments(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(user.AuthLevel > 1);

            ValidationHelper.ValidateEntity(await _userRepository.UserExists(id), "felhasználó");

            var doctor = await _userRepository.GetUserByIdAsync(id);
            ValidationHelper.ValidateEntity(doctor.AuthLevel > 1, "doktor");

            return _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetDoctorInactiveAppointments(id));
        }


        public async Task ValidateUser(DataErrorException error, AddAppointmentDto appointment, string ownerId)
        {
            ValidationHelper.ValidateEntity(await _userRepository.UserExists(ownerId), "felhasználó");
            var user = await _userRepository.GetUserByIdAsync(ownerId);
            ValidationHelper.ValidateEntity(await _userRepository.UserExists(appointment.DoctorId), "felhasználó");
            ValidationHelper.ValidateEntity((await _userRepository.GetUserByIdAsync(appointment.DoctorId)).AuthLevel > 1, "doktor");

            if (appointment.AnimalId != null)
            {
                ValidationHelper.ValidateEntity(await _animalRepository.AnimalExists((int)appointment.AnimalId), "állat");
                var animal = await _animalRepository.GetAnimalByIdAsync((int)appointment.AnimalId);
                ValidationHelper.ValidatePermission(animal.OwnerId == ownerId || user.AuthLevel > 1);
            }
        }

        public async Task ValidateAppointment(DataErrorException error, AddAppointmentDto appointment, string ownerId)
        {
            ValidationHelper.ValidateEntity(await _treatmentRepository.TreatmentExists(appointment.TreatmentId), "kezelés");

            var treatment = await _treatmentRepository.GetTreatmentByIdAsync(appointment.TreatmentId);
            ValidationHelper.ValidateData(error, treatment.DoctorId == appointment.DoctorId, "treatmentId", "A megadott kezelés nem a megadott orvos azonosítóhoz tartozik.");

            var treatmentTime = (await _treatmentRepository.GetTreatmentTimesByTreatmentIdAsync(treatment.Id))
                                .Where(tt => tt.DayOfWeek == ((int)appointment.StartDate.DayOfWeek))
                                .Where(tt => appointment.StartDate.GetMinutesLocalTime() >= tt.GetStartInMinutes() && appointment.EndDate.GetMinutesLocalTime() <= tt.GetEndInMinutes()
                                ).FirstOrDefault();

            ValidationHelper.ValidateData(error, treatmentTime != null, "treatmentId", "A megadott időpont nem megfelelő.");
            ValidationHelper.ValidateData(error, appointment.StartDate < appointment.EndDate, "date", "Rossz időpont intervallum. Kezdés később van, mint a befejezés.");
            ValidationHelper.ValidateData(error, appointment.StartDate >= DateTime.Now.ToLocalTime(), "date", "Rossz időpont. Az időpont kezdete nem lehet korábban mint az aktuális idő.");

            var expectedDuration = appointment.EndDate - appointment.StartDate;
            ValidationHelper.ValidateData(error, expectedDuration.Minutes % treatmentTime.Duration == 0, "date", "Rossz időpont. A megadott időpont nincs a választható időpontok között.");

            var reserved = (await this.GetReservedTimes(appointment.StartDate.ToLocalTime(), appointment.DoctorId))
                            .Where(rt => (rt.StartDate <= appointment.StartDate.ToLocalTime() && rt.EndDate > appointment.StartDate.ToLocalTime())
                                || (rt.StartDate >= appointment.StartDate.ToLocalTime() && rt.EndDate < appointment.EndDate.ToLocalTime())
                                || (rt.StartDate < appointment.EndDate.ToLocalTime() && rt.EndDate > appointment.EndDate.ToLocalTime())
                                || (rt.StartDate <= appointment.StartDate.ToLocalTime() && rt.EndDate > appointment.EndDate.ToLocalTime())
                                ).ToList();

            ValidationHelper.ValidateData(error, reserved.Count == 0, "date", "A megadott időpont már foglalt.");
        }

    }
}
