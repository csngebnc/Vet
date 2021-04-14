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
    public class TherapiaManager
    {
        private readonly ITherapiaRepository _therapiaRepository;
        private readonly IMapper _mapper;

        public TherapiaManager(IMapper mapper, ITherapiaRepository therapiaRepository)
        {
            _therapiaRepository = therapiaRepository;
            _mapper = mapper;
        }

        public async Task<TherapiaDto> AddTherapia(AddTherapiaDto therapiaDto)
        {
            var therapia = _mapper.Map<Therapia>(therapiaDto);
            return _mapper.Map<TherapiaDto>(await _therapiaRepository.AddTherapia(therapia));
        }

        public async Task<TherapiaDto> UpdateTherapia(Therapia therapia)
        {
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
            var therapia = await _therapiaRepository.GetTherapiaById(id);
            if (therapia.TherapyRecords.Count > 0) return false;
            return await _therapiaRepository.DeleteTherapia(therapia);
        }

        public async Task ChageStateOfTherapia(int id)
        {
            var therapia = await _therapiaRepository.GetTherapiaById(id);
            therapia.IsInactive = !therapia.IsInactive;
            await _therapiaRepository.UpdateTherapia(therapia);
        }

        public async Task<bool> TherapiaExists(int id)
            => (await _therapiaRepository.GetTherapiaById(id)) != null;

        public async Task<IEnumerable<TherapiaDto>> GetTherapias()
            => _mapper.Map<IEnumerable<TherapiaDto>>(await _therapiaRepository.GetTherapias());

        public async Task<TherapiaDto> GetTherapiaById(int id)
            => _mapper.Map<TherapiaDto>(await _therapiaRepository.GetTherapiaById(id));
    }
}
