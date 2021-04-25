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
    public class TherapiaManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly ITherapiaRepository _therapiaRepository;
        private readonly IMapper _mapper;

        public TherapiaManager(IMapper mapper, ITherapiaRepository therapiaRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _therapiaRepository = therapiaRepository;
            _mapper = mapper;
        }

        public async Task<TherapiaDto> AddTherapia(AddTherapiaDto therapiaDto)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 2, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            var therapia = _mapper.Map<Therapia>(therapiaDto);
            return _mapper.Map<TherapiaDto>(await _therapiaRepository.AddTherapia(therapia));
        }

        public async Task<TherapiaDto> UpdateTherapia(Therapia therapia)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 2, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            ValidationHelper.ValidateData(error, await _therapiaRepository.TherapiaExists(therapia.Id), "therapiaId", "A megadott azonosítóval nem létezik terápia.");

            var _therapia = await _therapiaRepository.GetTherapiaById(therapia.Id);
            _therapia.Name = therapia.Name;
            _therapia.Unit = therapia.Unit;
            _therapia.UnitName = therapia.UnitName;
            _therapia.PricePerUnit = therapia.PricePerUnit;
            _therapia.IsInactive = therapia.IsInactive;
            return _mapper.Map<TherapiaDto>(await _therapiaRepository.UpdateTherapia(_therapia));
        }

        public async Task<bool> DeleteTherapia(int id)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 2, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            ValidationHelper.ValidateData(error, await _therapiaRepository.TherapiaExists(id), "therapiaId", "A megadott azonosítóval nem létezik terápia.");

            var therapia = await _therapiaRepository.GetTherapiaById(id);
            ValidationHelper.ValidateData(error, therapia.TherapyRecords.Count == 0, "therapiaId", "A kezelés nem törölhető, mert már van kórlap, amelyen szerepel.");
            return await _therapiaRepository.DeleteTherapia(therapia);
        }

        public async Task ChageStateOfTherapia(int id)
        {
            var error = new DataErrorException();
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateData(error, loggedInUser.AuthLevel > 2, "userId", "Nincs jogosultságod a művelet végrehajtásához.");
            ValidationHelper.ValidateData(error, await _therapiaRepository.TherapiaExists(id), "therapiaId", "A megadott azonosítóval nem létezik terápia.");

            var therapia = await _therapiaRepository.GetTherapiaById(id);
            therapia.IsInactive = !therapia.IsInactive;
            await _therapiaRepository.UpdateTherapia(therapia);
        }

        public async Task<IEnumerable<TherapiaDto>> GetTherapias()
            => _mapper.Map<IEnumerable<TherapiaDto>>(await _therapiaRepository.GetTherapias());

        public async Task<TherapiaDto> GetTherapiaById(int id)
            => _mapper.Map<TherapiaDto>(await _therapiaRepository.GetTherapiaById(id));
    }
}
