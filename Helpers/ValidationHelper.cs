using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL.Exceptions;

namespace Vet.Helpers
{
    public static class ValidationHelper
    {
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
