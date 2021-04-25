using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL;
using Vet.Models.DTOs;

namespace Vet.GraphQL
{
    public class Mutation
    {
        public async Task<VetUserDto> PromoteUser([Service] DoctorManager _doctorManager ,string email)
            => await _doctorManager.PromoteToDoctor(email);

        public async Task<VetUserDto> DemoteDoctor([Service] DoctorManager _doctorManager, string id)
            => await _doctorManager.DemoteToUser(id);

    }
}
