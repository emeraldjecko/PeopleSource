using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;

namespace PeoplesSource.Attribute
{
    /// <summary>
    /// Class ValidatePasswordLengthAttribute. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// The default error message
        /// </summary>
        private const string DefaultErrorMessage = "'{0}' must be at least {1} characters long.";
        /// <summary>
        /// The _min characters
        /// </summary>
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatePasswordLengthAttribute"/> class.
        /// </summary>
        public ValidatePasswordLengthAttribute()
            : base(DefaultErrorMessage)
        {
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>An instance of the formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>true if the specified value is valid; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            var valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
}
