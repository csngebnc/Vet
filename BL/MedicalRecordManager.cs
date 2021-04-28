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
using Vet.BL.Exceptions;
using Vet.Helpers;
using Vet.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Vet.BL
{
    public class MedicalRecordManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMedicalRecordRepository _recordRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITherapiaRepository _therapiaRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IPhotoManager _photoManager;
        private readonly IMapper _mapper;

        private readonly PdfManager _pdf;

        public MedicalRecordManager(IMapper mapper, PdfManager p, IMedicalRecordRepository recordRepository, IUserRepository userRepository, ITherapiaRepository therapiaRepository, IAnimalRepository animalRepository, IPhotoManager photoManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _recordRepository = recordRepository;
            _userRepository = userRepository;
            _therapiaRepository = therapiaRepository;
            _animalRepository = animalRepository;
            _photoManager = photoManager;
            _mapper = mapper;
            _pdf = p;
        }

        public async Task<int> AddMedicalRecord(AddMedicalRecordDto recordDto)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);

            var medicalRecord = _mapper.Map<MedicalRecord>(recordDto);

            medicalRecord.OwnerId = await _userRepository.GetUserIdByUserEmail(recordDto.OwnerEmail);
            medicalRecord.DoctorId = loggedInUser.Id;
            await _recordRepository.AddMedicalRecord(medicalRecord);

            var therapiaRecords = recordDto.Therapias;
            foreach (var therapia in therapiaRecords)
            {
                ValidationHelper.ValidateEntity(await _therapiaRepository.TherapiaExists(therapia.TherapiaId), "terápia");
                var addTherapia = _mapper.Map<TherapiaRecord>(therapia);
                addTherapia.MedicalRecordId = medicalRecord.Id;
                await _recordRepository.AddTherapiaToMedicalRecord(addTherapia);
            }
            return medicalRecord.Id;
        }

        public async Task<MedicalRecordDto> UpdateMedicalRecord(UpdateMedicalRecordDto recordDto)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);

            ValidationHelper.ValidateEntity(await _recordRepository.MedicalRecordExists(recordDto.Id), "kórlap");
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
                ValidationHelper.ValidateEntity(await _therapiaRepository.TherapiaExists(therapia.TherapiaId), "terápia");
                var addTherapia = _mapper.Map<TherapiaRecord>(therapia);
                addTherapia.MedicalRecordId = _record.Id;
                await _recordRepository.AddTherapiaToMedicalRecord(addTherapia);
            }

            await _recordRepository.UpdateMedicalRecord(_record);
            return _mapper.Map<MedicalRecordDto>(_record);
        }

        // NON USED
        public async Task<bool> DeleteMedicalRecord(int id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1);

            ValidationHelper.ValidateEntity(await _recordRepository.MedicalRecordExists(id), "kórlap");
            var _record = await _recordRepository.GetMedicalRecordById(id);

            var therapias = await _recordRepository.GetTherapiaRecordsByRecordId(_record.Id);
            foreach (var trecord in therapias)
            {
                await _recordRepository.RemoveTherapiaFromMedicalRecord(trecord);
            }

            return await _recordRepository.DeleteMedicalRecord(_record);
        }

        public async Task<MedicalRecordDto> GetMedicalRecordById(int id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateEntity(await _recordRepository.MedicalRecordExists(id), "kórlap");
            var _record = await _recordRepository.GetMedicalRecordById(id);
            ValidationHelper.ValidatePermission(_record.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);
            return _mapper.Map<MedicalRecordDto>(_record);
        }
        public async Task<IEnumerable<MedicalRecordDto>> GetCurrentUserMedicalRecords()
            => _mapper.Map<IEnumerable<MedicalRecordDto>>(await _recordRepository.GetMedicalRecordsByUserId(_httpContextAccessor.GetCurrentUserId()));

        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByUserId(string id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateEntity(await _userRepository.UserExists(id), "felhasználó");
            ValidationHelper.ValidatePermission(id == loggedInUser.Id || loggedInUser.AuthLevel > 1);

            return _mapper.Map<IEnumerable<MedicalRecordDto>>(await _recordRepository.GetMedicalRecordsByUserId(id));
        }
        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByAnimalId(int id)
        {
            ValidationHelper.ValidateEntity(await _animalRepository.AnimalExists(id), "állat");
            var animal = await _animalRepository.GetAnimalByIdAsync(id);
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());

            ValidationHelper.ValidatePermission(animal.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);
            return _mapper.Map<IEnumerable<MedicalRecordDto>>(await _recordRepository.GetMedicalRecordsByAnimalId(id));
        }

        public async Task<bool> RemoveTherapiaFromMedicalRecord(int id)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1); ;

            ValidationHelper.ValidateEntity(await _recordRepository.TherapiaRecordExists(id), "kórlapra rögzített kezelés");
            return await _recordRepository.RemoveTherapiaFromMedicalRecord(await _recordRepository.GetTherapiaRecordById(id));
        }


        public async Task<bool> UploadPhoto(IFormFile image, int medId)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1); ;

            ValidationHelper.ValidateEntity(await _recordRepository.MedicalRecordExists(medId), "kórlap");

            var path = await _photoManager.UploadMedicalRecordPhoto(image);

            return await _recordRepository.AddPhoto(new MedicalRecordPhoto { MedicalRecordId = medId, Path = path}); 
        }

        public async Task<bool> DeletePhoto(int photoId)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidatePermission(loggedInUser.AuthLevel > 1); ;
            ValidationHelper.ValidateEntity(await _recordRepository.MedicalRecordPhotoExists(photoId), "kórlaphoz rögzített kép");

            var photo = await _recordRepository.GetMedicalRecordPhotoById(photoId);
            if (await _recordRepository.RemovePhoto(photo))
            {
                return _photoManager.RemovePhoto(photo.Path);
            }
            return false;
        }

        public async Task<byte[]> GeneratePdf(int id, string path)
        {
            var loggedInUser = await _userRepository.GetUserByIdAsync(_httpContextAccessor.GetCurrentUserId());
            ValidationHelper.ValidateEntity(await _recordRepository.MedicalRecordExists(id), "kórlap");
            var record = await _recordRepository.GetMedicalRecordById(id);
            ValidationHelper.ValidatePermission(record.OwnerId == loggedInUser.Id || loggedInUser.AuthLevel > 1);

            var net = new System.Net.WebClient();
            return net.DownloadData(await _pdf.GeneratePdf(path + "/" + id, record) + ".pdf");
        }
    }
}
