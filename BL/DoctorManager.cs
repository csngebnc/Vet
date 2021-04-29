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
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);

            var doctorId = await _userRepository.GetUserIdByUserEmail(email);
            ValidationHelper.ValidateEntity(await _userRepository.UserExists(doctorId), "felhasználó");

            var doctor = await _userRepository.GetUserByIdAsync(doctorId);
            ValidationHelper.ValidateEntityAlreadyExists(doctor.AuthLevel < 2, "A megadott azonosítóval rendelkező felhasználó már orvos.");

            return _mapper.Map<VetUserDto>(await _doctorRepository.PromoteToDoctor(await _doctorRepository.GetDoctorByEmail(email)));
        }

        public async Task<VetUserDto> DemoteToUser(string id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidatePermission(id != loggedInUser.Id);
            ValidationHelper.ValidateEntity(await _userRepository.UserExists(id), "felhasználó");
            var doctor = await _userRepository.GetUserByIdAsync(id);
            ValidationHelper.ValidateEntityAlreadyExists(doctor.AuthLevel > 1, "A megadott azonosítóval rendelkező felhasználó nem orvos.");

            return _mapper.Map<VetUserDto>(await _doctorRepository.DemoteToUser(await _doctorRepository.GetDoctorById(id)));
        }

        public async Task<IEnumerable<VetUserDto>> GetDoctors()
            => _mapper.Map<IEnumerable<VetUserDto>>(await _doctorRepository.GetDoctors());

        public async Task<HolidayDto> AddHoliday(AddHolidayDto holiday)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidateData(new DataErrorException(), holiday.StartDate.ToLocalTime() <= holiday.EndDate.ToLocalTime(), "date", "A kezdés nem lehet később, mint a szabadság vége.");

            var _holiday = _mapper.Map<Holiday>(holiday);
            _holiday.DoctorId = loggedInUser.Id;
            return _mapper.Map<HolidayDto>(await _doctorRepository.AddHoliday(_holiday));
        }

        public async Task<HolidayDto> EditHoliday(HolidayDto holiday)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);
            ValidationHelper.ValidateData(new DataErrorException(), holiday.StartDate.ToLocalTime() <= holiday.EndDate.ToLocalTime(), "date", "A kezdés nem lehet később, mint a szabadság vége.");
            ValidationHelper.ValidateEntity(await _doctorRepository.HolidayExists(holiday.Id), "szabadság");

            var holidayOwner = (await _doctorRepository.GetHolidayById(holiday.Id)).DoctorId;
            ValidationHelper.ValidatePermission(loggedInUser.Id == holidayOwner || loggedInUser.AuthLevel > 2);
            
            return _mapper.Map<HolidayDto>(await _doctorRepository.EditHoliday(_mapper.Map<Holiday>(holiday)));
        }

        public async Task<bool> DeleteHoliday(int id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);

            ValidationHelper.ValidateEntity(await _doctorRepository.HolidayExists(id), "szabadság"); ;
            var holidayOwner = (await _doctorRepository.GetHolidayById(id)).DoctorId;

            ValidationHelper.ValidatePermission(loggedInUser.Id == holidayOwner || loggedInUser.AuthLevel > 2);

            return await _doctorRepository.DeleteHoliday(await _doctorRepository.GetHolidayById(id));
        }

        public async Task<HolidayDto> GetHolidayById(int id)
        {
            ValidationHelper.ValidateEntity(await _doctorRepository.HolidayExists(id), "szabadság");
            return _mapper.Map<HolidayDto>(await _doctorRepository.GetHolidayById(id));
        }

        public async Task<IEnumerable<HolidayDto>> GetDoctorsHolidays(string doctorId)
        {
            ValidationHelper.ValidateEntity(await _userRepository.UserExists(doctorId), "felhasználó");
            var user = await _userRepository.GetUserByIdAsync(doctorId);
            ValidationHelper.ValidateEntity(user.AuthLevel > 2, "orvos");
            return _mapper.Map<IEnumerable<HolidayDto>>(await _doctorRepository.GetDoctorsHolidays(doctorId));
        }

        public async Task<IEnumerable<HolidayDto>> GetHolidays()
            => _mapper.Map<IEnumerable<HolidayDto>>(await _doctorRepository.GetHolidays());
    }
}
