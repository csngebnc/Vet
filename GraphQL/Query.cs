using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL;
using Vet.Extensions;
using Vet.Models.DTOs;

namespace Vet.GraphQL
{
    // kb. mint egy controller
    public class Query
    {
        public async Task<IEnumerable<VetUserDto>> GetDoctors([Service] DoctorManager _doctorManager)
            => await _doctorManager.GetDoctors();
    }
}
