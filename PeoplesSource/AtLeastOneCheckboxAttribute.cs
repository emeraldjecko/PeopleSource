using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace PeoplesSource
{
    public class AtLeastOneCheckboxAttribute : ValidationAttribute
    {
        private string[] _checkboxNames;
        private String ErrorMessage { get; set; }
        public AtLeastOneCheckboxAttribute(String errormessage, params string[] checkboxNames)
        {
            _checkboxNames = checkboxNames;
            ErrorMessage = errormessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInfos = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => _checkboxNames.Contains(x.Name));

            var values = propertyInfos.Select(x => x.GetGetMethod().Invoke(value, null));
            if (values.Any(Convert.ToBoolean))
                return ValidationResult.Success;
            //ErrorMessage = "You must select at least one tobbaco use";
            return new ValidationResult(ErrorMessage);
        }
    }
}