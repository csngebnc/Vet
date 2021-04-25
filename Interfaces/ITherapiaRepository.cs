using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Models;

namespace Vet.Interfaces
{
    public interface ITherapiaRepository
    {
        Task<Therapia> AddTherapia(Therapia therapia);
        Task<Therapia> UpdateTherapia(Therapia therapia);
        Task<bool> DeleteTherapia(Therapia therapia);

        Task<IEnumerable<Therapia>> GetTherapias();
        Task<Therapia> GetTherapiaById(int id);
        Task<bool> TherapiaExists(int therapiaId);
    }
}
