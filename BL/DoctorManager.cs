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
    public class DoctorManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorManager(IMapper mapper, IUserRepository userRepository, IDoctorRepository doctorRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _doctorRepository = doctorRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<VetUserDto> PromoteToDoctor(string email)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            var doctorId = await _userRepository.GetUserIdByUserEmail(email);
            ValidationHelper.ValidateData(error, await _userRepository.UserExists(doctorId), "doctorId", "A megadott e-mail címmel nem létezik felhasználó.");
            var doctor = await _userRepository.GetUserByIdAsync(doctorId);
            ValidationHelper.ValidateData(error, doctor.AuthLevel < 2, "doctorId", "A megadott e-maillel már van doktor rögzítve.");
            return _mapper.Map<VetUserDto>(await _doctorRepository.PromoteToDoctor(await _doctorRepository.GetDoctorByEmail(email)));
        }

        public async Task<VetUserDto> DemoteToUser(string id)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            ValidationHelper.ValidateData(error, await _userRepository.UserExists(id), "doctorId", "A megadott e-mail címmel nem létezik felhasználó.");
            var doctor = await _userRepository.GetUserByIdAsync(id);
            ValidationHelper.ValidateData(error, doctor.AuthLevel > 1, "doctorId", "A megadott e-maillel nincs rögzített doktor.");

            return _mapper.Map<VetUserDto>(await _doctorRepository.DemoteToUser(await _doctorRepository.GetDoctorById(id)));
        }

        public async Task<IEnumerable<VetUserDto>> GetDoctors()
            => _mapper.Map<IEnumerable<VetUserDto>>(await _doctorRepository.GetDoctors());

        public async Task<HolidayDto> AddHoliday(AddHolidayDto holiday)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            var _holiday = _mapper.Map<Holiday>(holiday);
            _holiday.DoctorId = loggedInUser.Id;
            return _mapper.Map<HolidayDto>(await _doctorRepository.AddHoliday(_holiday));
        }
        public async Task<HolidayDto> EditHoliday(HolidayDto holiday)
        {
            var error = new DataErrorException();

            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            ValidationHelper.ValidateData(error, await _doctorRepository.HolidayExists(holiday.Id), "holidayId", "A megadott azonosítóval nincs szabadság rögzítve.");
            var holidayOwner = (await _doctorRepository.GetHolidayById(holiday.Id)).DoctorId;

            ValidationHelper.ValidateData(error, loggedInUser.Id == holidayOwner || loggedInUser.AuthLevel > 2, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            
            return _mapper.Map<HolidayDto>(await _doctorRepository.EditHoliday(_mapper.Map<Holiday>(holiday)));
        }
        public async Task<bool> DeleteHoliday(int id)
        {
            var error = new DataErrorException();

            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 1, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            ValidationHelper.ValidateData(error, await _doctorRepository.HolidayExists(id), "holidayId", "A megadott azonosítóval nincs szabadság rögzítve.");
            var holidayOwner = (await _doctorRepository.GetHolidayById(id)).DoctorId;

            ValidationHelper.ValidateData(error, loggedInUser.Id == holidayOwner || loggedInUser.AuthLevel > 2, "userId", "Nincs jogosultságod a művelet végrehajtásához.");

            return await _doctorRepository.DeleteHoliday(await _doctorRepository.GetHolidayById(id));
        }

        public async Task<HolidayDto> GetHolidayById(int id)
            => _mapper.Map<HolidayDto>(await _doctorRepository.GetHolidayById(id));
        public async Task<IEnumerable<HolidayDto>> GetDoctorsHolidays(string doctorId)
            => _mapper.Map<IEnumerable<HolidayDto>>(await _doctorRepository.GetDoctorsHolidays(doctorId));
        public async Task<IEnumerable<HolidayDto>> GetHolidays()
            => _mapper.Map<IEnumerable<HolidayDto>>(await _doctorRepository.GetHolidays());
    }
}
