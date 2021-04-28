using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL.Exceptions;
using Vet.BL.ProblemDetails.Exceptions;

namespace Vet.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidateEntity(bool condition, string entityName)
        {
            if (!condition)
            {
                throw new EntityNotFoundException($"A megadott azonosítóval nem található {entityName} a rendszerben.");
            }
        }

        public static void ValidateEntityAlreadyExists(bool condition, string message)
        {
            if (!condition)
            {
                throw new ExistingEntityException(message);
            }
        }

        public static void ValidatePermission(bool condition)
        {
            if (!condition)
            {
                throw new PermissionDeniedException("Nincs jogosultságod a művelet végrehajtásához.");
            }
        }

        public static void ValidateData(DataErrorException error, bool condition, string key, string message)
        {
            if (!condition)
            {
                error.AddError(key, message);
                throw error;
            }
        }
    }
}
