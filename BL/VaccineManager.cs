using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs.Vaccine;
using Vet.BL.Exceptions;
using Vet.Helpers;
using Vet.Extensions;

namespace Vet.BL
{
    public class VaccineManager
    {
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IMapper _mapper;

        public VaccineManager(IMapper mapper, IVaccineRepository vaccineRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IAnimalRepository animalRepository)
        {
            _vaccineRepository = vaccineRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _animalRepository = animalRepository;
            _mapper = mapper;
        }

        public async Task<VaccineDto> AddVaccine(AddVaccineDto vaccine)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 2);
            var _vaccine = _mapper.Map<Vaccine>(vaccine);
            return _mapper.Map<VaccineDto>(await _vaccineRepository.AddVaccine(_vaccine));
        }

        public async Task<VaccineDto> UpdateVaccine(VaccineDto vaccine)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 2);
            ValidationHelper.ValidateEntity(await _vaccineRepository.VaccineExists(vaccine.Id), "oltás");
            return _mapper.Map<VaccineDto>(await _vaccineRepository.UpdateVaccine(_mapper.Map<Vaccine>(vaccine)));
        }

        public async Task<bool> DeleteVaccine(int id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 2);
            ValidationHelper.ValidateEntity(await _vaccineRepository.VaccineExists(id), "oltás");
            var _vaccine = await _vaccineRepository.GetVaccineById(id);
            ValidationHelper.ValidateEntityAlreadyExists(_vaccine.Records.Count == 0, "Sikertelen törlés, az oltást már legalább egy állatnál rögzítették.");
            return await _vaccineRepository.DeleteVaccine(_vaccine);
        }

        public async Task<VaccineRecordDto> AddVaccineRecord(AddVaccineRecordDto record)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, record.Date.ToLocalTime() <= DateTime.Now, "date", "Az oltás időpontja nem lehet a jövőben.");
            ValidationHelper.ValidateEntity(await _animalRepository.AnimalExists(record.AnimalId), "állat");
            ValidationHelper.ValidateEntity(await _vaccineRepository.VaccineExists(record.VaccineId), "oltás");

            var _animal = await _animalRepository.GetAnimalByIdAsync(record.AnimalId);
            ValidationHelper.ValidatePermission(_animal.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);

            var _record = _mapper.Map<VaccineRecord>(record);

            return _mapper.Map<VaccineRecordDto>(await _vaccineRepository.AddVaccineRecord(_record));
        }

        public async Task<VaccineRecordDto> UpdateVaccineRekord(UpdateVaccineRecordDto record)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateEntity(await _vaccineRepository.VaccineRecordExists(record.Id), "korábban beadott oltás");


            ValidationHelper.ValidateData(error, record.Date.ToLocalTime() <= DateTime.Now, "date", "Az oltás időpontja nem lehet a jövőben.");
            ValidationHelper.ValidateEntity(await _animalRepository.AnimalExists(record.AnimalId), "állat");

            var _animal = await _animalRepository.GetAnimalByIdAsync(record.AnimalId);
            ValidationHelper.ValidatePermission(_animal.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);
            var _record = await _vaccineRepository.GetVaccineRecordById(record.Id);
            _record.Date = record.Date.ToLocalTime();
            return _mapper.Map<VaccineRecordDto>(await _vaccineRepository.UpdateVaccineRecord(_record));
        }

        public async Task<bool> DeleteVaccineRecord(int id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateEntity(await _vaccineRepository.VaccineRecordExists(id), "korábban beadott oltás");
            var _record = await _vaccineRepository.GetVaccineRecordById(id);
            ValidationHelper.ValidatePermission(_record.Animal.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);

            return await _vaccineRepository.DeleteVaccineRecord(_record);
        }

        public async Task<IEnumerable<VaccineDto>> GetVaccines()
            => _mapper.Map<IEnumerable<VaccineDto>>(await _vaccineRepository.GetVaccines());

        public async Task<VaccineDto> GetVaccineById(int id)
        {
            ValidationHelper.ValidateEntity(await _vaccineRepository.VaccineExists(id), "oltás");
            return _mapper.Map<VaccineDto>(await _vaccineRepository.GetVaccineById(id));
        }

        public async Task<IEnumerable<VaccineRecordDto>> GetVaccineRecordsOfAnimal(int animalId)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateEntity(await _animalRepository.AnimalExists(animalId), "állat");
            var _animal = await _animalRepository.GetAnimalByIdAsync(animalId);
            ValidationHelper.ValidatePermission(_animal.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);
            return _mapper.Map<IEnumerable<VaccineRecordDto>>(await _vaccineRepository.GetVaccineRecordsOfAnimal(animalId));
        }

        public async Task<VaccineRecordDto> GetVaccineRecordById(int id)
        {
            var error = new DataErrorException();
            ValidationHelper.ValidateEntity(await _vaccineRepository.VaccineRecordExists(id), "korábban beadott oltás");
            var _record = await _vaccineRepository.GetVaccineRecordById(id);
            var owner = await _userRepository.GetUserByIdAsync(_record.Animal.OwnerId);
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(owner.Id == loggedInUser.Id || loggedInUser.AuthLevel > 1);

            return _mapper.Map<VaccineRecordDto>(_record);
        }

    }
}
