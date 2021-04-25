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
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            var _treatment = _mapper.Map<Treatment>(treatment);
            _treatment.DoctorId = loggedInUser.Id;
            return _mapper.Map<TreatmentDto>(await _treatmentRepository.AddTreatment(_treatment));
        }

        public async Task<TreatmentDto> UpdateTreatment(UpdateTreatmentDto treatment)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            ValidationHelper.ValidateData(error, await _treatmentRepository.TreatmentExists(treatment.Id), "treatmentId", "A megadott azonosítóval nem létezik kezelés.");

            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(treatment.Id);

            ValidationHelper.ValidateData(error, _treatment.DoctorId == loggedInUser.Id || loggedInUser.AuthLevel > 2, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            _treatment = _mapper.Map<Treatment>(treatment);
            return _mapper.Map<TreatmentDto>(await _treatmentRepository.UpdateTreatment(_treatment));
        }

        public async Task<bool> DeleteTreatment(int id)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            ValidationHelper.ValidateData(error, await _treatmentRepository.TreatmentExists(id), "treatmentId", "A megadott azonosítóval nem létezik kezelés.");
            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(id);
            ValidationHelper.ValidateData(error, _treatment.TreatmentTimes.Count == 0, "treatmentId", "Sikertelen törlés, a megadott kezeléshez tartoznak idősávok.");
            return await _treatmentRepository.DeleteTreatment(id);
        }

        public async Task ChageStateOfTreatment(int id)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            ValidationHelper.ValidateData(error, await _treatmentRepository.TreatmentExists(id), "treatmentId", "A megadott azonosítóval nem létezik kezelés.");
            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(id);
            ValidationHelper.ValidateData(error, _treatment.DoctorId == loggedInUser.Id || loggedInUser.AuthLevel > 2, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            _treatment.IsInactive = !_treatment.IsInactive;
            await _treatmentRepository.UpdateTreatment(_treatment);
        }

        public async Task<IEnumerable<TreatmentDto>> GetAllTreatments()
            => _mapper.Map<IEnumerable<TreatmentDto>>(await _treatmentRepository.GetTreatmentsAsync());

        public async Task<IEnumerable<TreatmentDto>> GetMyTreatments(string doctorId) 
            => _mapper.Map<IEnumerable<TreatmentDto>>(await _treatmentRepository.GetTreatmentsByDoctorIdAsync(doctorId));

        public async Task<TreatmentDto> GetTreatmentByIdAsync(int id)
        {
            ValidationHelper.ValidateData(new DataErrorException(), await _treatmentRepository.TreatmentExists(id), "treamentId", "A megadott azonosítóval nem létezik kezelés.");
            return _mapper.Map<TreatmentDto>(await _treatmentRepository.GetTreatmentByIdAsync(id));
        }

        public async Task<IEnumerable<TreatmentDto>> GetTreatmentsByDoctorId(string id)
        {
            ValidationHelper.ValidateData(new DataErrorException(), await _userRepository.UserExists(id), "userId", "A megadott azonosítóval nem létezik felhasználó.");
            return _mapper.Map<IEnumerable<TreatmentDto>>(await _treatmentRepository.GetTreatmentsByDoctorIdAsync(id));
        }


        public async Task<TreatmentTimeDto> AddTreatmentTime(AddTreatmentTimeDto time)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            ValidationHelper.ValidateData(error, await _treatmentRepository.TreatmentExists(time.TreatmentId), "treatmentId", "A megadott azonosítóval nem létezik kezelés.");
            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(time.TreatmentId);
            ValidationHelper.ValidateData(error, _treatment.DoctorId == loggedInUser.Id || loggedInUser.AuthLevel > 2, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            var _treatmentTime = _mapper.Map<TreatmentTime>(time);
            return _mapper.Map<TreatmentTimeDto>(await _treatmentRepository.AddTreatmentTime(_treatmentTime));
        }

        public async Task<bool> DeleteTreatmentTime(int timeId)
            => await _treatmentRepository.DeleteTreatmentTime(await _treatmentRepository.GetTreatmentTimeByIdAsync(timeId));

        public async Task<TreatmentTimeDto> UpdateTreatmentTime(UpdateTreatmentTimeDto time)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            ValidationHelper.ValidateData(error, await _treatmentRepository.TreatmentTimeExists(time.Id), "treatmenttimeId", "A megadott azonosítóval nem létezik kezelési idősáv.");
            ValidationHelper.ValidateData(error, await _treatmentRepository.TreatmentExists(time.TreatmentId), "treatmentId", "A megadott azonosítóval nem létezik kezelés.");
            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(time.TreatmentId);
            ValidationHelper.ValidateData(error, _treatment.DoctorId == loggedInUser.Id || loggedInUser.AuthLevel > 2, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            var _treatmentTime = await _treatmentRepository.GetTreatmentTimeByIdAsync(time.Id);
            _treatmentTime = _mapper.Map<TreatmentTime>(time);
            return _mapper.Map<TreatmentTimeDto>(await _treatmentRepository.UpdateTreatmentTime(_treatmentTime));
        }

        public async Task ChageStateOfTreatmentTime(int id)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            ValidationHelper.ValidateData(error, await _treatmentRepository.TreatmentTimeExists(id), "treatmenttimeId", "A megadott azonosítóval nem létezik kezelési idősáv.");
            var treatmentTime = await _treatmentRepository.GetTreatmentTimeByIdAsync(id);

            ValidationHelper.ValidateData(error, await _treatmentRepository.TreatmentExists(treatmentTime.TreatmentId), "treatmentId", "A megadott azonosítóval nem létezik kezelés.");
            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(treatmentTime.TreatmentId);
            ValidationHelper.ValidateData(error, _treatment.DoctorId == loggedInUser.Id || loggedInUser.AuthLevel > 2, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            treatmentTime.IsInactive = !treatmentTime.IsInactive;
            await _treatmentRepository.UpdateTreatmentTime(treatmentTime);
        }

        public async Task<TreatmentTimeDto> GetTreatmentTimeByIdAsync(int id)
            => _mapper.Map<TreatmentTimeDto>(await _treatmentRepository.GetTreatmentTimeByIdAsync(id));
        public async Task<IEnumerable<TreatmentTimeDto>> GetTreatmentTimesByTreatmentIdAsync(int id)
            => _mapper.Map<IEnumerable<TreatmentTimeDto>>(await _treatmentRepository.GetTreatmentTimesByTreatmentIdAsync(id));
    }
}
