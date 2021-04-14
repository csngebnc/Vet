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
    public class TreatmentManager
    {
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IMapper _mapper;

        public TreatmentManager(IMapper mapper, ITreatmentRepository treatmentRepository)
        {
            _treatmentRepository = treatmentRepository;
            _mapper = mapper;
        }
        
        public async Task<TreatmentDto> AddTreatment(AddTreatmentDto treatment, string doctorId)
        {
            var _treatment = _mapper.Map<Treatment>(treatment);
            _treatment.DoctorId = doctorId;
            return _mapper.Map<TreatmentDto>(await _treatmentRepository.AddTreatment(_treatment));
        }

        public async Task<TreatmentDto> UpdateTreatment(UpdateTreatmentDto treatment)
        {
            var _treatment = await _treatmentRepository.GetTreatmentByIdAsync(treatment.Id);
            _treatment = _mapper.Map<Treatment>(treatment);
            return _mapper.Map<TreatmentDto>(await _treatmentRepository.UpdateTreatment(_treatment));
        }

        public async Task<bool> DeleteTreatment(int id)
            => await _treatmentRepository.DeleteTreatment(id);

        public async Task ChageStateOfTreatment(int id)
        {
            var treatment = await _treatmentRepository.GetTreatmentByIdAsync(id);
            treatment.IsInactive = !treatment.IsInactive;
            await _treatmentRepository.UpdateTreatment(treatment);
        }

        public async Task<IEnumerable<TreatmentDto>> GetAllTreatments()
            => _mapper.Map<IEnumerable<TreatmentDto>>(await _treatmentRepository.GetTreatmentsAsync());

        public async Task<IEnumerable<TreatmentDto>> GetMyTreatments(string doctorId) 
            => _mapper.Map<IEnumerable<TreatmentDto>>(await _treatmentRepository.GetTreatmentsByDoctorIdAsync(doctorId));

        public async Task<TreatmentDto> GetTreatmentByIdAsync(int id)
            => _mapper.Map<TreatmentDto>(await _treatmentRepository.GetTreatmentByIdAsync(id));

        public async Task<IEnumerable<TreatmentDto>> GetTreatmentsByDoctorId(string id)
            => _mapper.Map<IEnumerable<TreatmentDto>>(await _treatmentRepository.GetTreatmentsByDoctorIdAsync(id));


        public async Task<TreatmentTimeDto> AddTreatmentTime(AddTreatmentTimeDto time)
        {
            var _treatmentTime = _mapper.Map<TreatmentTime>(time);
            return _mapper.Map<TreatmentTimeDto>(await _treatmentRepository.AddTreatmentTime(_treatmentTime));
        }

        public async Task<bool> DeleteTreatmentTime(int timeId)
            => await _treatmentRepository.DeleteTreatmentTime(await _treatmentRepository.GetTreatmentTimeByIdAsync(timeId));

        public async Task<TreatmentTimeDto> UpdateTreatmentTime(UpdateTreatmentTimeDto time)
        {
            var _treatmentTime = await _treatmentRepository.GetTreatmentTimeByIdAsync(time.Id);
            _treatmentTime = _mapper.Map<TreatmentTime>(time);
            return _mapper.Map<TreatmentTimeDto>(await _treatmentRepository.UpdateTreatmentTime(_treatmentTime));
        }

        public async Task ChageStateOfTreatmentTime(int id)
        {
            var treatmentTime = await _treatmentRepository.GetTreatmentTimeByIdAsync(id);
            treatmentTime.IsInactive = !treatmentTime.IsInactive;
            await _treatmentRepository.UpdateTreatmentTime(treatmentTime);
        }

        public async Task<TreatmentTimeDto> GetTreatmentTimeByIdAsync(int id)
            => _mapper.Map<TreatmentTimeDto>(await _treatmentRepository.GetTreatmentTimeByIdAsync(id));
        public async Task<IEnumerable<TreatmentTimeDto>> GetTreatmentTimesByTreatmentIdAsync(int id)
            => _mapper.Map<IEnumerable<TreatmentTimeDto>>(await _treatmentRepository.GetTreatmentTimesByTreatmentIdAsync(id));
    }
}
