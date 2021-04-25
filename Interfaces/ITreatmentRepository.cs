using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Models;

namespace Vet.Interfaces
{
    public interface ITreatmentRepository
    {
        Task<Treatment> AddTreatment(Treatment treatment);
        Task<Treatment> UpdateTreatment(Treatment treatment);
        Task<bool> DeleteTreatment(int id);

        Task<IEnumerable<Treatment>> GetTreatmentsAsync();
        Task<Treatment> GetTreatmentByIdAsync(int id);
        Task<IEnumerable<Treatment>> GetTreatmentsByDoctorIdAsync(string id);

        //////////////////////////////////////////////
        ///

        Task<TreatmentTime> AddTreatmentTime(TreatmentTime time);
        Task<TreatmentTime> UpdateTreatmentTime(TreatmentTime time);
        Task<bool> DeleteTreatmentTime(TreatmentTime time);

        Task<TreatmentTime> GetTreatmentTimeByIdAsync(int id);
        Task<IEnumerable<TreatmentTime>> GetTreatmentTimesByTreatmentIdAsync(int id);

        Task<bool> TreatmentExists(int treatmentId);
        Task<bool> TreatmentTimeExists(int treatmentimeId);


    }
}
