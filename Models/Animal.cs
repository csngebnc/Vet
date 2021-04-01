using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vet.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double? Weight { get; set; }
        public string Sex { get; set; }
        [ForeignKey("Species")]
        public int SpeciesId { get; set; }
        public AnimalSpecies Species { get; set; }
        public string? SubSpecies { get; set; }
        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        public VetUser Owner { get; set; }
        public string? PhotoPath { get; set; }
        public bool IsArchived { get; set; }

        public ICollection<MedicalRecord> MedicalRecords { get; set; }
        public ICollection<VaccineRecord> VaccineRecords { get; set; }
    }
}
