using System;
using System.Collections.Generic;
using System.Linq;

namespace  PeoplesSource.Extensions
{
    /// <summary>
    /// Class DecimalExtensions.
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Percentages the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="total">The total.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal Percentage(this IList<decimal> list, decimal total)
        {
            var sum = list.Sum();
            return sum.Percentage(total);
        }

        /// <summary>
        /// Percentages the specified part.
        /// </summary>
        /// <param name="part">The part.</param>
        /// <param name="total">The total.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal Percentage(this decimal part, decimal total)
        {
            if (total == 0 || part == 0)
                return 0;
            return (part / total) * 100;
        }

        /// <summary>
        /// To the numeric string.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>System.String.</returns>
        public static string ToNumericString(this decimal number)
        {
            return ((decimal?)number).ToNumericString();
        }

        /// <summary>
        /// To the numeric string.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>System.String.</returns>
        public static string ToNumericString(this decimal? number)
        {
            return number.ToNumericString(0, false, true, false);
        }

        /// <summary>
        /// To the numeric string.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <param name="removeTrailingZeros">if set to <c>true</c> [remove trailing zeros].</param>
        /// <param name="addCommmas">if set to <c>true</c> [add commmas].</param>
        /// <param name="nullReturnsEmptyString">if set to <c>true</c> [null returns empty string].</param>
        /// <returns>System.String.</returns>
        public static string ToNumericString(this decimal number, int decimalPlaces, bool removeTrailingZeros, bool addCommmas, bool nullReturnsEmptyString)
        {
            return ((decimal?)number).ToNumericString(decimalPlaces, removeTrailingZeros, addCommmas, nullReturnsEmptyString);
        }

        /// <summary>
        /// To the numeric string.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <param name="removeTrailingZeros">if set to <c>true</c> [remove trailing zeros].</param>
        /// <param name="addCommmas">if set to <c>true</c> [add commmas].</param>
        /// <param name="nullReturnsEmptyString">if set to <c>true</c> [null returns empty string].</param>
        /// <returns>System.String.</returns>
        public static string ToNumericString(this decimal? number, int decimalPlaces, bool removeTrailingZeros, bool addCommmas, bool nullReturnsEmptyString)
        {
            if (nullReturnsEmptyString && number == null)
            {
                return string.Empty;
            }
            if (number == null)
            {
                number = 0;
            }

            number = Math.Round(number.Value, decimalPlaces);

            string numberString;

            if (addCommmas && number > 0)
            {
                numberString = number.Value.ToString("###,###,###,###.####");
            }
            else
            {
                numberString = number.ToString();
            }

            if (removeTrailingZeros && numberString.Contains("."))
            {
                numberString = numberString.TrimEnd('0');
                numberString = numberString.TrimEnd('.');
            }

            return numberString;
        }
    }
}