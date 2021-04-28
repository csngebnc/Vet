using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.BL.ProblemDetails.Exceptions
{
    public class ExistingEntityException : Exception
    {
        public ExistingEntityException()
        {
        }

        public ExistingEntityException(string message)
            : base(message)
        {
        }

        public ExistingEntityException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
