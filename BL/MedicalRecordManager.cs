using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Interfaces;
using Vet.Models;
using Vet.Models.DTOs;
using Vet.Models.DTOs.MedicalRecord;

namespace Vet.BL
{
    public class MedicalRecordManager
    {
        private readonly IMedicalRecordRepository _recordRepository; 
        private readonly IUserRepository _userRepository;
        private readonly IPhotoManager _photoManager;
        private readonly IMapper _mapper;

        public MedicalRecordManager(IMapper mapper, IMedicalRecordRepository recordRepository, IUserRepository userRepository, IPhotoManager photoManager)
        {
            _recordRepository = recordRepository;
            _userRepository = userRepository;
            _photoManager = photoManager;
            _mapper = mapper;
        }

        public async Task<int> AddMedicalRecord(AddMedicalRecordDto recordDto, string doctorId)
        {
            var medicalRecord = _mapper.Map<MedicalRecord>(recordDto);
            medicalRecord.OwnerId = await _userRepository.GetUserIdByUserEmail(recordDto.OwnerEmail);
            medicalRecord.DoctorId = doctorId;
            await _recordRepository.AddMedicalRecord(medicalRecord);

            var therapiaRecords = recordDto.Therapias;
            foreach (var therapia in therapiaRecords)
            {
                var addTherapia = _mapper.Map<TherapiaRecord>(therapia);
                addTherapia.MedicalRecordId = medicalRecord.Id;
                await _recordRepository.AddTherapiaToMedicalRecord(addTherapia);
            }
            return medicalRecord.Id;
        }

        public async Task<MedicalRecordDto> UpdateMedicalRecord(UpdateMedicalRecordDto recordDto)
        {
            var _record = await _recordRepository.GetMedicalRecordById(recordDto.Id);
            _record.OwnerEmail = recordDto.OwnerEmail;
            _record.OwnerId = await _userRepository.GetUserIdByUserEmail(recordDto.OwnerEmail);
            _record.AnimalId = recordDto.AnimalId;
            _record.Anamnesis = recordDto.Anamnesis;
            _record.Symptoma = recordDto.Symptoma;
            _record.Details = recordDto.Details;

            var therapiaRecords = recordDto.Therapias;
            foreach (var therapia in therapiaRecords)
            {
                var addTherapia = _mapper.Map<TherapiaRecord>(therapia);
                addTherapia.MedicalRecordId = _record.Id;
                await _recordRepository.AddTherapiaToMedicalRecord(addTherapia);
            }

            await _recordRepository.UpdateMedicalRecord(_record);
            return _mapper.Map<MedicalRecordDto>(_record);
        }

        public async Task<bool> DeleteMedicalRecord(int id)
        {
            var _record = await _recordRepository.GetMedicalRecordById(id);

            var therapias = await _recordRepository.GetTherapiaRecordsByRecordId(_record.Id);
            foreach (var trecord in therapias)
            {
                await _recordRepository.RemoveTherapiaFromMedicalRecord(trecord);
            }

            return await _recordRepository.DeleteMedicalRecord(_record);
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecords()
            => _mapper.Map<IEnumerable<MedicalRecordDto>>(await _recordRepository.GetMedicalRecords());
        public async Task<MedicalRecordDto> GetMedicalRecordById(int id)
            => _mapper.Map<MedicalRecordDto>(await _recordRepository.GetMedicalRecordById(id));
        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByUserEmail(string email)
            => _mapper.Map<IEnumerable<MedicalRecordDto>>(await _recordRepository.GetMedicalRecordsByUserEmail(email));
        public async Task<IEnumerable<MedicalRecordDto>> GetCurrentUserMedicalRecords(string id)
            => _mapper.Map<IEnumerable<MedicalRecordDto>>(await _recordRepository.GetMedicalRecordsByUserId(id));
        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByUserId(string id)
            => _mapper.Map<IEnumerable<MedicalRecordDto>>(await _recordRepository.GetMedicalRecordsByUserId(id));
        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByAnimalId(int id)
            => _mapper.Map<IEnumerable<MedicalRecordDto>>(await _recordRepository.GetMedicalRecordsByAnimalId(id));
        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByDoctorId(string id)
            => _mapper.Map<IEnumerable<MedicalRecordDto>>(await _recordRepository.GetMedicalRecordsByDoctorId(id));

        public async Task<bool> AddTherapiaToMedicalRecord(AddTherapiaToRecordDto therapia)
            => await _recordRepository.AddTherapiaToMedicalRecord(_mapper.Map<TherapiaRecord>(therapia));

        public async Task<TherapiaRecordDto> UpdateTherapiaToMedicalRecord(UpdateTherapiaOnRecordDto therapiaRecord)
        {
            var _record = await _recordRepository.GetTherapiaRecordById(therapiaRecord.Id);
            _record = _mapper.Map<TherapiaRecord>(therapiaRecord);
            return _mapper.Map<TherapiaRecordDto>(await _recordRepository.UpdateTherapiaOnMedicalRecord(_record));
        }

        public async Task<bool> RemoveTherapiaFromMedicalRecord(int id)
            => await _recordRepository.RemoveTherapiaFromMedicalRecord(await _recordRepository.GetTherapiaRecordById(id));


        public async Task<bool> UploadPhoto(IFormFile image, int medId)
        {
            var path = await _photoManager.UploadMedicalRecordPhoto(image);

            return await _recordRepository.AddPhoto(new MedicalRecordPhoto { MedicalRecordId = medId, Path = path}); 
        }


        public async Task<bool> DeletePhoto(int photoId)
        {
            var photo = await _recordRepository.GetMedicalRecordPhotoById(photoId);
            if (await _recordRepository.RemovePhoto(photo))
            {
                return _photoManager.RemovePhoto(photo.Path);
            }
            return false;
        }
    }
}
