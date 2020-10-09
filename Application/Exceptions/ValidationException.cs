using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Exceptions
{
    public class ValidationException : Exception
    {
        
        public ValidationException(string code) : base("One or more validation failures have occurred.")
        {
            Errors = new List<string>();
            this.Code = code;
        }
        public List<string> Errors { get; }
        public string Code { get; set; }
        public ValidationException(string code,IEnumerable<ValidationFailure> failures)
            : this(code)
        {
            foreach (var failure in failures)
            {
                Errors.Add(failure.ErrorMessage);
            }
        }

    }
}
