using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL.Exceptions;
using Vet.Extensions;
using Vet.Helpers;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.BL
{
    public class TreatmentManager
    {
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public TreatmentManager(IMapper mapper, ITreatmentRepository treatmentRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _treatmentRepository = treatmentRepository;
            _mapper = mapper;
        }
        
        public async Task<TreatmentDto> AddTreatment(AddTreatmentDto treatment)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);

            var _treatment = _mapper.Map<Treatment>(treatment);
            _treatment.DoctorId = loggedInUser.Id;
            return _mapper.Map<TreatmentDto>(await _treatmentRepository.AddTreatment(_treatment));
        }

        public async Task<TreatmentDto> UpdateTreatment(UpdateTreatmentDto treatment)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidateEntity(await _treatmentRepository.TreatmentExists(treatment.Id), "kezelés");

            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(treatment.Id);

            ValidationHelper.ValidatePermission(_treatment.DoctorId == loggedInUser.Id || loggedInUser.AuthLevel > 2);
            _treatment = _mapper.Map<Treatment>(treatment);
            return _mapper.Map<TreatmentDto>(await _treatmentRepository.UpdateTreatment(_treatment));
        }

        public async Task<bool> DeleteTreatment(int id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidateEntity(await _treatmentRepository.TreatmentExists(id), "kezelés");
            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(id);
            ValidationHelper.ValidateEntityAlreadyExists(_treatment.TreatmentTimes.Count == 0, "Sikertelen törlés, a megadott kezeléshez tartoznak idősávok.");
            return await _treatmentRepository.DeleteTreatment(id);
        }

        public async Task ChageStateOfTreatment(int id)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidateEntity(await _treatmentRepository.TreatmentExists(id), "kezelés");
            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(id);
            ValidationHelper.ValidatePermission(_treatment.DoctorId == loggedInUser.Id || loggedInUser.AuthLevel > 2);

            _treatment.IsInactive = !_treatment.IsInactive;
            await _treatmentRepository.UpdateTreatment(_treatment);
        }

        public async Task<IEnumerable<TreatmentDto>> GetAllTreatments()
            => _mapper.Map<IEnumerable<TreatmentDto>>(await _treatmentRepository.GetTreatmentsAsync());

        public async Task<IEnumerable<TreatmentDto>> GetMyTreatments(string doctorId) 
            => _mapper.Map<IEnumerable<TreatmentDto>>(await _treatmentRepository.GetTreatmentsByDoctorIdAsync(doctorId));

        public async Task<TreatmentDto> GetTreatmentByIdAsync(int id)
        {
            ValidationHelper.ValidateEntity(await _treatmentRepository.TreatmentExists(id), "kezelés");
            return _mapper.Map<TreatmentDto>(await _treatmentRepository.GetTreatmentByIdAsync(id));
        }

        public async Task<IEnumerable<TreatmentDto>> GetTreatmentsByDoctorId(string id)
        {
            ValidationHelper.ValidateEntity(await _userRepository.UserExists(id), "felhasználó");
            var user = await _userRepository.GetUserByIdAsync(id);
            ValidationHelper.ValidateEntity(user.AuthLevel > 1, "doktor");
            return _mapper.Map<IEnumerable<TreatmentDto>>(await _treatmentRepository.GetTreatmentsByDoctorIdAsync(id));
        }


        public async Task<TreatmentTimeDto> AddTreatmentTime(AddTreatmentTimeDto time)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidateEntity(await _treatmentRepository.TreatmentExists(time.TreatmentId), "kezelés");
            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(time.TreatmentId);
            ValidationHelper.ValidatePermission(_treatment.DoctorId == loggedInUser.Id || loggedInUser.AuthLevel > 2);

            var _treatmentTime = _mapper.Map<TreatmentTime>(time);
            return _mapper.Map<TreatmentTimeDto>(await _treatmentRepository.AddTreatmentTime(_treatmentTime));
        }

        public async Task<bool> DeleteTreatmentTime(int timeId)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidateEntity(await _treatmentRepository.TreatmentTimeExists(timeId), "kezelési idősáv");
            var _time = await _treatmentRepository.GetTreatmentTimeByIdAsync(timeId);
            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(_time.TreatmentId);
            ValidationHelper.ValidatePermission(_treatment.DoctorId == loggedInUser.Id || loggedInUser.AuthLevel > 2);

            return await _treatmentRepository.DeleteTreatmentTime(await _treatmentRepository.GetTreatmentTimeByIdAsync(timeId));
        }

        public async Task<TreatmentTimeDto> UpdateTreatmentTime(UpdateTreatmentTimeDto time)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidateEntity(await _treatmentRepository.TreatmentTimeExists(time.Id), "kezelési idősáv");
            ValidationHelper.ValidateEntity(await _treatmentRepository.TreatmentExists(time.TreatmentId), "kezelés");
            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(time.TreatmentId);
            ValidationHelper.ValidatePermission(_treatment.DoctorId == loggedInUser.Id || loggedInUser.AuthLevel > 2);

            var _treatmentTime = await _treatmentRepository.GetTreatmentTimeByIdAsync(time.Id);
            _treatmentTime = _mapper.Map<TreatmentTime>(time);
            return _mapper.Map<TreatmentTimeDto>(await _treatmentRepository.UpdateTreatmentTime(_treatmentTime));
        }

        public async Task ChageStateOfTreatmentTime(int id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidateEntity(await _treatmentRepository.TreatmentTimeExists(id), "kezelési idősáv");
            var treatmentTime = await _treatmentRepository.GetTreatmentTimeByIdAsync(id);

            ValidationHelper.ValidateEntity(await _treatmentRepository.TreatmentExists(treatmentTime.TreatmentId), "kezelés");
            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(treatmentTime.TreatmentId);
            ValidationHelper.ValidatePermission(_treatment.DoctorId == loggedInUser.Id || loggedInUser.AuthLevel > 2);

            treatmentTime.IsInactive = !treatmentTime.IsInactive;
            await _treatmentRepository.UpdateTreatmentTime(treatmentTime);
        }

        public async Task<TreatmentTimeDto> GetTreatmentTimeByIdAsync(int id)
            => _mapper.Map<TreatmentTimeDto>(await _treatmentRepository.GetTreatmentTimeByIdAsync(id));
        public async Task<IEnumerable<TreatmentTimeDto>> GetTreatmentTimesByTreatmentIdAsync(int id)
            => _mapper.Map<IEnumerable<TreatmentTimeDto>>(await _treatmentRepository.GetTreatmentTimesByTreatmentIdAsync(id));
    }
}
