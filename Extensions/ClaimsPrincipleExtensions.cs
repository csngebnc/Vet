using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Vet.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetById(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
