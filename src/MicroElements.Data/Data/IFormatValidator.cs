using System;

namespace MicroElements.Data
{
    using System.Collections.Generic;

    public interface IFormatValidator
    {
        IEnumerable<ValidationResult> Validate(DataContainer dataContainer);
    }


    //see IValidatableObject
    public class ValidationResult
    {
        public string ErrorMessage { get; }

        public IEnumerable<string> MemberNames { get; }

        public ValidationResult(string errorMessage)
            : this(errorMessage, null)
        {
        }

        public ValidationResult(string errorMessage, IEnumerable<string> memberNames)
        {
            ErrorMessage = errorMessage;
            MemberNames = memberNames ?? Array.Empty<string>();
        }

        public override string ToString()
        {
            return ErrorMessage ?? base.ToString();
        }
    }
}
