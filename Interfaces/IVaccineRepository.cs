﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Models;

namespace Vet.Interfaces
{
    public interface IVaccineRepository
    {
        Task<Vaccine> AddVaccine(Vaccine vaccine);
        Task<Vaccine> UpdateVaccine(Vaccine vaccine);
        Task<bool> DeleteVaccine(Vaccine vaccine);

        ///////

        Task<VaccineRecord> AddVaccineRecord(VaccineRecord record);
        Task<VaccineRecord> UpdateVaccineRecord(VaccineRecord record);
        Task<bool> DeleteVaccineRecord(VaccineRecord record);

        Task<IEnumerable<Vaccine>> GetVaccines();
        Task<Vaccine> GetVaccineById(int id);
        Task<IEnumerable<VaccineRecord>> GetVaccineRecordsOfAnimal(int animalId);
        Task<VaccineRecord> GetVaccineRecordById(int id);

        Task<bool> VaccineExists(int vaccinerecordId);
        Task<bool> VaccineRecordExists(int vaccinerecordId);
    }
}
