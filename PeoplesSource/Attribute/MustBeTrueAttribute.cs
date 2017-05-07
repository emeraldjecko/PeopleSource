using System;
using System.ComponentModel.DataAnnotations;

namespace PeoplesSource.Attribute
{
    /// <summary>
    /// Class MustBeTrueAttribute.
    /// </summary>
    public class MustBeTrueAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>true if the specified value is valid; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            try
            {
                bool newVal = Convert.ToBoolean(value);
                if (newVal)
                    return true;
                return false;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }

}