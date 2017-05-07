using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PeoplesSource.Attribute
{
    /// <summary>
    /// Class PropertiesMustMatchAttribute. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        /// <summary>
        /// The default error message
        /// </summary>
        private const string DefaultErrorMessage = "'{0}' and '{1}' do not match.";
        /// <summary>
        /// The _type identifier
        /// </summary>
        private readonly object _typeId = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesMustMatchAttribute"/> class.
        /// </summary>
        /// <param name="originalProperty">The original property.</param>
        /// <param name="confirmProperty">The confirm property.</param>
        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(DefaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        /// <summary>
        /// Gets the confirm property.
        /// </summary>
        /// <value>The confirm property.</value>
        public string ConfirmProperty { get; private set; }
        /// <summary>
        /// Gets the original property.
        /// </summary>
        /// <value>The original property.</value>
        public string OriginalProperty { get; private set; }

        /// <summary>
        /// When implemented in a derived class, gets a unique identifier for this <see cref="T:System.Attribute" />.
        /// </summary>
        /// <value>The type identifier.</value>
        /// <returns>An <see cref="T:System.Object" /> that is a unique identifier for the attribute.</returns>
        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>An instance of the formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>true if the specified value is valid; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            var properties = TypeDescriptor.GetProperties(value);
            var originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            var confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Equals(originalValue, confirmValue);
        }
    }
}
