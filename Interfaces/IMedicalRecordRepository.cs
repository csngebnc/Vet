using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Models;
using Vet.Models.DTOs;
using Vet.Models.DTOs.MedicalRecord;

namespace Vet.Interfaces
{
    public interface IMedicalRecordRepository
    {
        Task<bool> AddMedicalRecord(MedicalRecord recordDto);
        Task<MedicalRecord> UpdateMedicalRecord(MedicalRecord spec);
        Task<bool> DeleteMedicalRecord(MedicalRecord spec);

        Task<bool> AddPhoto(MedicalRecordPhoto photoRecord);
        Task<bool> RemovePhoto(MedicalRecordPhoto photoRecord);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecords();
        Task<MedicalRecord> GetMedicalRecordById(int id);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByUserEmail(string email);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByUserId(string Id);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByAnimalId(int id);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByDoctorId(string id);
        /// /////////////////////

        Task<bool> AddTherapiaToMedicalRecord(TherapiaRecord therapia);
        Task<TherapiaRecord> UpdateTherapiaOnMedicalRecord(TherapiaRecord therapiaRecord);
        Task<bool> RemoveTherapiaFromMedicalRecord(TherapiaRecord therapia);

        Task<IEnumerable<TherapiaRecord>> GetTherapiaRecordsByRecordId(int id);
        Task<TherapiaRecord> GetTherapiaRecordById(int id);

        Task<MedicalRecordPhoto> GetMedicalRecordPhotoById(int photoId);

        Task<bool> MedicalRecordExists(int medicalrecordId);
        Task<bool> MedicalRecordPhotoExists(int photoId);
        Task<bool> TherapiaRecordExists(int therapiaRecordId);
    }

}
