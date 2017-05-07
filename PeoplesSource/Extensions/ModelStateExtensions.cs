using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace  PeoplesSource.Extensions
{
    /// <summary>
    /// Class ModelStateExtensions.
    /// </summary>
    public static class ModelStateExtensions
    {
        /// <summary>
        /// Gets the error messages.
        /// </summary>
        /// <param name="modelStateDictionary">The model state dictionary.</param>
        /// <returns>IList{System.String}.</returns>
        public static IList<string> GetErrorMessages(this ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary.Values
                            .Where(v => v.Errors != null && v.Errors.Count > 0)
                            .SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                            .ToList();
        }

        /// <summary>
        /// Gets the validation summary.
        /// </summary>
        /// <param name="modelStateDictionary">The model state dictionary.</param>
        /// <returns>System.String.</returns>
        public static string GetValidationSummary(this ModelStateDictionary modelStateDictionary)
        {
            return string.Join("<br>", modelStateDictionary.GetErrorMessages().ToArray());
        }

    }
}