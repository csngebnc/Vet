using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.BL.ProblemDetails
{
    public class DataErrorProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public DataErrorProblemDetails()
        {
            this.Errors = new Dictionary<string, ICollection<string>>();
        }
        public Dictionary<string, ICollection<string>> Errors { get; set; }
    }
}
