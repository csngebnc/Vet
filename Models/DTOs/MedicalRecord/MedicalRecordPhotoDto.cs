using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models.DTOs.MedicalRecord
{
    public class MedicalRecordPhotoDto
    {
        public int Id { get; set; }
        public int MedicalRecordId { get; set; }
        public string Path { get; set; }
    }
}
