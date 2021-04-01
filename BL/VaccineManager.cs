using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs.Vaccine;

namespace Vet.BL
{
    public class VaccineManager
    {
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IMapper _mapper;

        public VaccineManager(IMapper mapper, IVaccineRepository vaccineRepository)
        {
            _vaccineRepository = vaccineRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddVaccine(AddVaccineDto vaccine)
        {
            var _vaccine = _mapper.Map<Vaccine>(vaccine);
            return await _vaccineRepository.AddVaccine(_vaccine);
        }

        public async Task<VaccineDto> UpdateVaccine(VaccineDto vaccine)
        {
            var _vaccine = await _vaccineRepository.GetVaccineById(vaccine.Id);
            return _mapper.Map<VaccineDto>(await _vaccineRepository.UpdateVaccine(_mapper.Map<Vaccine>(vaccine)));
        }

        public async Task<bool> DeleteVaccine(int id)
        {
            var _vaccine = await _vaccineRepository.GetVaccineById(id);
            return await _vaccineRepository.DeleteVaccine(_vaccine);
        }

        public async Task<bool> AddVaccineRecord(AddVaccineRecordDto record)
        {
            var _record = _mapper.Map<VaccineRecord>(record);
            return await _vaccineRepository.AddVaccineRecord(_record);
        }

        public async Task<VaccineRecordDto> UpdateVaccineRekord(UpdateVaccineRecordDto record)
        {
            var _record = await _vaccineRepository.GetVaccineRecordById(record.Id);
            _record.Date = record.Date;
            return _mapper.Map<VaccineRecordDto>(await _vaccineRepository.UpdateVaccineRecord(_record));
        }

        public async Task<bool> DeleteVaccineRecord(int id)
        {
            var _record = await _vaccineRepository.GetVaccineRecordById(id);
            return await _vaccineRepository.DeleteVaccineRecord(_record);
        }

        public async Task<IEnumerable<VaccineDto>> GetVaccines()
            => _mapper.Map<IEnumerable<VaccineDto>>(await _vaccineRepository.GetVaccines());

        public async Task<VaccineDto> GetVaccineById(int id)
            => _mapper.Map<VaccineDto>(await _vaccineRepository.GetVaccineById(id));

        public async Task<IEnumerable<VaccineRecordDto>> GetVaccineRecordsOfAnimal(int animalId)
            => _mapper.Map<IEnumerable<VaccineRecordDto>>(await _vaccineRepository.GetVaccineRecordsOfAnimal(animalId));

        public async Task<VaccineRecordDto> GetVaccineRecordById(int id)
            => _mapper.Map<VaccineRecordDto>(await _vaccineRepository.GetVaccineRecordById(id));

    }
}
