using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.BL.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        private string detail;
        public string Detail { get { return detail; } }

        public EntityNotFoundException(string message, string detail = "")
            : base(message)
        {
            this.detail = detail;
        }

        public EntityNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
