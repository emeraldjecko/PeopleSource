using System.Web.Mvc;
using PeoplesSource.Extensions;

namespace PeoplesSource.Helpers
{
    /// <summary>
    /// Class NumericHtmlHelper.
    /// </summary>
    public static class NumericHtmlHelper
    {

        /// <summary>
        /// Numerics the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="number">The number.</param>
        /// <returns>System.String.</returns>
        public static string Numeric(this HtmlHelper helper, decimal? number)
        {
            return Numeric(helper, number, 0, true, true, false);
        }

        /// <summary>
        /// Numerics the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="number">The number.</param>
        /// <param name="nullDefaultText">The null default text.</param>
        /// <returns></returns>
        public static string Numeric(this HtmlHelper helper, decimal? number, string nullDefaultText)
        {
            return Numeric(helper, number, 0, true, true, true, nullDefaultText);
        }

        /// <summary>
        /// Numerics the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="number">The number.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <param name="nullDefaultText">The null default text.</param>
        /// <returns></returns>
        public static string Numeric(this HtmlHelper helper, decimal? number, int decimalPlaces, string nullDefaultText)
        {
            return Numeric(helper, number, decimalPlaces, true, true, true, nullDefaultText);
        }

        /// <summary>
        /// Numerics the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="number">The number.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <returns>System.String.</returns>
        public static string Numeric(this HtmlHelper helper, decimal? number, int decimalPlaces)
        {
            return Numeric(helper, number, decimalPlaces, true, true, false);
        }

     
        /// <summary>
        /// Numerics the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="number">The number.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <param name="removeTrailingZeros">if set to <c>true</c> [remove trailing zeros].</param>
        /// <param name="addCommmas">if set to <c>true</c> [add commmas].</param>
        /// <returns>System.String.</returns>
        public static string Numeric(this HtmlHelper helper, decimal? number, int decimalPlaces, bool removeTrailingZeros, bool addCommmas)
        {
            return Numeric(helper, number, decimalPlaces, removeTrailingZeros, addCommmas, false);
        }

        /// <summary>
        /// Numerics for input.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="number">The number.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <returns>System.String.</returns>
        public static string NumericForInput(this HtmlHelper helper, decimal? number, int decimalPlaces)
        {
            return Numeric(helper, number, decimalPlaces, true, false, true);
        }

        /// <summary>
        /// Numerics the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="number">The number.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <param name="removeTrailingZeros">if set to <c>true</c> [remove trailing zeros].</param>
        /// <param name="addCommmas">if set to <c>true</c> [add commmas].</param>
        /// <param name="nullReturnsEmptyString">if set to <c>true</c> [null returns empty string].</param>
        /// <returns>System.String.</returns>
        public static string Numeric(this HtmlHelper helper, decimal? number, int decimalPlaces, bool removeTrailingZeros, bool addCommmas, bool nullReturnsEmptyString)
        {
            return number.ToNumericString(decimalPlaces, removeTrailingZeros, addCommmas, nullReturnsEmptyString);
        }

        public static string Numeric(this HtmlHelper helper, decimal? number, int decimalPlaces, bool removeTrailingZeros, bool addCommmas, bool nullReturnsEmptyString, string nullDefaultText)
        {
            return number.ToNumericString(decimalPlaces, removeTrailingZeros, addCommmas, nullReturnsEmptyString);
        }

    
        /// <summary>
        /// Suffixes the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="number">The number.</param>
        /// <param name="pluralSuffix">The plural suffix.</param>
        /// <param name="singularSuffix">The singular suffix.</param>
        /// <returns>System.String.</returns>
        public static string Suffix(this HtmlHelper helper, decimal? number, string pluralSuffix, string singularSuffix)
        {
            if (number == 0)
            {
                return "";
            }
            return number > 1 ? pluralSuffix : singularSuffix;
        }
    }
}
