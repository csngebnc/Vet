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
    public class DoctorManager
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorManager(IMapper mapper, IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<VetUserDto> PromoteToDoctor(string email)
            => _mapper.Map<VetUserDto>(await _doctorRepository.PromoteToDoctor(await _doctorRepository.GetDoctorByEmail(email)));

        public async Task<VetUserDto> DemoteToUser(string id)
            => _mapper.Map<VetUserDto>(await _doctorRepository.DemoteToUser(await _doctorRepository.GetDoctorById(id)));

        public async Task<IEnumerable<VetUserDto>> GetDoctors()
            => _mapper.Map<IEnumerable<VetUserDto>>(await _doctorRepository.GetDoctors());

        public async Task<HolidayDto> AddHoliday(AddHolidayDto holiday, string doctorId)
        {
            var _holiday = _mapper.Map<Holiday>(holiday);
            _holiday.DoctorId = doctorId;
            return _mapper.Map<HolidayDto>(await _doctorRepository.AddHoliday(_holiday));
        }
        public async Task<HolidayDto> EditHoliday(HolidayDto holiday)
        {
            return _mapper.Map<HolidayDto>(await _doctorRepository.EditHoliday(_mapper.Map<Holiday>(holiday)));
        }
        public async Task<bool> DeleteHoliday(int id)
            => await _doctorRepository.DeleteHoliday(await _doctorRepository.GetHolidayById(id));

        public async Task<HolidayDto> GetHolidayById(int id)
            => _mapper.Map<HolidayDto>(await _doctorRepository.GetHolidayById(id));
        public async Task<IEnumerable<HolidayDto>> GetDoctorsHolidays(string doctorId)
            => _mapper.Map<IEnumerable<HolidayDto>>(await _doctorRepository.GetDoctorsHolidays(doctorId));
        public async Task<IEnumerable<HolidayDto>> GetHolidays()
            => _mapper.Map<IEnumerable<HolidayDto>>(await _doctorRepository.GetHolidays());
    }
}
