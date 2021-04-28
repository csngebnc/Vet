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
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 2);
            var therapia = _mapper.Map<Therapia>(therapiaDto);
            return _mapper.Map<TherapiaDto>(await _therapiaRepository.AddTherapia(therapia));
        }

        public async Task<TherapiaDto> UpdateTherapia(Therapia therapia)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 2);
            ValidationHelper.ValidateEntity(await _therapiaRepository.TherapiaExists(therapia.Id), "terápia");

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
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 2);
            ValidationHelper.ValidateEntity(await _therapiaRepository.TherapiaExists(id), "terápia");

            var therapia = await _therapiaRepository.GetTherapiaById(id);
            ValidationHelper.ValidateEntityAlreadyExists(therapia.TherapyRecords.Count == 0, "A kezelés nem törölhető, mert már van kórlap, amelyen szerepel.");
            return await _therapiaRepository.DeleteTherapia(therapia);
        }

        public async Task ChageStateOfTherapia(int id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 2);
            ValidationHelper.ValidateEntity(await _therapiaRepository.TherapiaExists(id), "terápia");

            var therapia = await _therapiaRepository.GetTherapiaById(id);
            therapia.IsInactive = !therapia.IsInactive;
            await _therapiaRepository.UpdateTherapia(therapia);
        }

        public async Task<IEnumerable<TherapiaDto>> GetTherapias()
            => _mapper.Map<IEnumerable<TherapiaDto>>(await _therapiaRepository.GetTherapias());

        public async Task<TherapiaDto> GetTherapiaById(int id)
        {
            ValidationHelper.ValidateEntity(await _therapiaRepository.TherapiaExists(id), "terápia");
            return _mapper.Map<TherapiaDto>(await _therapiaRepository.GetTherapiaById(id));
        }
    }
}
