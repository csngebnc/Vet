using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.BL.ProblemDetails
{
    public class EntityNotFoundProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public string Key { get; set; }
        public string Details { get; set; }

    }
}
