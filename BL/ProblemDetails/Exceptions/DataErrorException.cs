using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.BL.Exceptions
{
    public class DataErrorException : Exception
    {
        public Dictionary<string,ICollection<string>> Errors { get; private set; }

        public DataErrorException()
        {
            this.Errors = new Dictionary<string, ICollection<string>>();
        }

        public DataErrorException(string message) : base(message)
        {
            this.Errors = new Dictionary<string, ICollection<string>>();
        }

        public DataErrorException(string message, Exception inner)
            : base(message, inner)
        {
            this.Errors = new Dictionary<string, ICollection<string>>();
        }

        public void AddError(string key, string message)
        {
            if (Errors.ContainsKey(key))
            {
                var errors = this.Errors[key];
                errors.Add(message);
                this.Errors[key] = errors;
            }
            else
            {
                var errors = new List<string>();
                errors.Add(message);
                this.Errors.Add(key, errors);
            }
        }
    }
}
