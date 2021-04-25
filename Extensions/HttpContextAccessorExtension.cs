using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Vet.Extensions
{
    public static class HttpContextAccessorExtension
    {
        public static string GetCurrentUserId(this IHttpContextAccessor httpContextAccessor)
            => httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
