using System.Linq;
using System.Security.Claims;

namespace Vet.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetById(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
