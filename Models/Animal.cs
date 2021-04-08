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
        #nullable enable
        public double? Weight { get; set; }
        #nullable disable
        public string Sex { get; set; }
        [ForeignKey("Species")]
        public int SpeciesId { get; set; }
        public AnimalSpecies Species { get; set; }
        #nullable enable
        public string? SubSpecies { get; set; }
        #nullable disable
        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        public VetUser Owner { get; set; }
        #nullable enable
        public string? PhotoPath { get; set; }
        #nullable disable
        public bool IsArchived { get; set; }

        public ICollection<MedicalRecord> MedicalRecords { get; set; }
        public ICollection<VaccineRecord> VaccineRecords { get; set; }
    }
}
