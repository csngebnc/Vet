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
            var error = new DataErrorException();

            ValidationHelper.ValidateData(error, await _appointmentRepository.AppointmentExists(id), "appointmentId", "A megadott azonosítóval nem létezik foglalt időpont.");
            var appointment = await _appointmentRepository.GetAppointmentById(id);

            ValidationHelper.ValidateData(error, await _userRepository.UserExists(_httpContextAccessor.GetCurrentUserId()), "userId", "A megadott azonosítóval nem létezik felhasználó.");
            var user = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, appointment.OwnerId == user.Id || user.AuthLevel > 1, "ownerId", "Nincs jogosultságod a művelet végrehajtásához.");

            return _mapper.Map<Appointment, AppointmentDto>(await _appointmentRepository.ResignAppointment(id, details));
        } 
        /*
        public async Task<IEnumerable<AppointmentDto>> GetAllAppointments()
            => _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetAllAppointments());
        public async Task<IEnumerable<AppointmentDto>> GetActiveAppointments()
            => _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetActiveAppointments());
        public async Task<IEnumerable<AppointmentDto>> GetInactiveAppointments()
            => _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetInactiveAppointments());
        */
        public async Task<IEnumerable<AppointmentTimeDto>> GetReservedTimes(DateTime time, string doctorId)
        {
            var error = new DataErrorException();

            ValidationHelper.ValidateData(error, await _userRepository.UserExists(doctorId), "doctorId", "A megadott azonosítóval nem létezik felhasználó.");
            var user = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, user.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            return _mapper.Map<IEnumerable<AppointmentTimeDto>>(await _appointmentRepository.GetReservedTimes(time, doctorId));
        }

        public async Task<IEnumerable<AppointmentDto>> GetUserAppointments(string id)
        {
            var error = new DataErrorException();
            ValidationHelper.ValidateData(error, await _userRepository.UserExists(id), "userId", "A megadott azonosítóval nem létezik felhasználó.");
            var user = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, _httpContextAccessor.GetCurrentUserId() == id || user.AuthLevel > 1, "ownerId", "Nincs jogosultságod a művelet végrehajtásához.");

            return _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetUserAppointments(id));
        }

        public async Task<IEnumerable<AppointmentDto>> GetDoctorActiveAppointments(string id)
        {
            var error = new DataErrorException();
            ValidationHelper.ValidateData(error, await _userRepository.UserExists(id), "doctorId", "A megadott azonosítóval nem létezik felhasználó.");
            var user = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, user.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            var doctor = await _userRepository.GetUserByIdAsync(id);
            ValidationHelper.ValidateData(error, user.AuthLevel > 1, "doctorId", "A megadott azonosítóval nincs doktor a rendszerben.");

            return _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetDoctorActiveAppointments(id));
        }
        public async Task<IEnumerable<AppointmentDto>> GetDoctorInactiveAppointments(string id)
        {
            var error = new DataErrorException();
            ValidationHelper.ValidateData(error, await _userRepository.UserExists(id), "doctorId", "A megadott azonosítóval nem létezik felhasználó.");
            var user = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, user.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            var doctor = await _userRepository.GetUserByIdAsync(id);
            ValidationHelper.ValidateData(error, user.AuthLevel > 1, "doctorId", "A megadott azonosítóval nincs doktor a rendszerben.");

            return _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetDoctorInactiveAppointments(id));
        }


        public async Task ValidateUser(DataErrorException error, AddAppointmentDto appointment, string ownerId)
        {
            ValidationHelper.ValidateData(error, (await _userRepository.UserExists(ownerId)), "ownerId", "A megadott azonosítóval nem létezik felhasználó.");

            if ((await _userRepository.UserExists(appointment.DoctorId)))
                ValidationHelper.ValidateData(error, (await _userRepository.GetUserByIdAsync(appointment.DoctorId)).AuthLevel > 1, "doctorId", "A megadott azonosítóval nincs doktor a rendszerben.");
            else
            {
                error.AddError("doctorId", "A megadott azonosítóval nincs doktor a rendszerben.");
                error.AddError("doctorId", "A megadott azonosítóval nincs doktor a rendszerben.");
                throw error;
            }

            if (appointment.AnimalId != null)
                if ((await _animalRepository.AnimalExists((int)appointment.AnimalId)))
                {
                    var animal = await _animalRepository.GetAnimalByIdAsync((int)appointment.AnimalId);
                    ValidationHelper.ValidateData(error, animal.OwnerId == ownerId, "animalId", "A megadott állat gazdája nem a bejelentkezett felhasználó.");
                }
        }

        public async Task ValidateAppointment(DataErrorException error, AddAppointmentDto appointment, string ownerId)
        {
            ValidationHelper.ValidateData(error, await _treatmentRepository.TreatmentExists(appointment.TreatmentId), "treatmentId", "A megadott kezelés nem létezik.");

            var treatment = await _treatmentRepository.GetTreatmentByIdAsync(appointment.TreatmentId);
            ValidationHelper.ValidateData(error, treatment.DoctorId == appointment.DoctorId, "treatmentId", "A megadott kezelés nem a megadott orvos azonosítóhoz tartozik.");

            var treatmentTime = (await _treatmentRepository.GetTreatmentTimesByTreatmentIdAsync(treatment.Id))
                                .Where(tt => tt.DayOfWeek == ((int)appointment.StartDate.DayOfWeek))
                                .Where(tt => appointment.StartDate.GetMinutesLocalTime() >= tt.GetStartInMinutes() && appointment.EndDate.GetMinutesLocalTime() <= tt.GetEndInMinutes()
                                ).FirstOrDefault();

            ValidationHelper.ValidateData(error, treatmentTime != null, "treatmentId", "A megadott időpont nem megfelelő.");
            ValidationHelper.ValidateData(error, appointment.StartDate < appointment.EndDate, "date", "Rossz időpont intervallum. Kezdés később van, mint a befejezés.");
            ValidationHelper.ValidateData(error, appointment.StartDate < DateTime.Now.ToLocalTime(), "date", "Rossz időpont. Az időpont kezdete nem lehet korábban mint az aktuális idő.");

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
