using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace PeoplesSource.Common
{
    public static class Extensions
    {
        #region Validation

        /// <summary>
        /// Validates that <paramref name="instance"/> is not null
        /// </summary>
        /// <typeparam name="TValidate">Type of the instance to validate</typeparam>
        /// <param name="instance">Instance to validate</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>Instance (allows one line assignment)</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="instance"/> is null.
        /// </exception>
        public static TValidate ThrowIfNull<TValidate>(this TValidate instance, string argumentName)
            where TValidate : class
        {
            if (instance == null)
            {
                throw new ArgumentNullException(argumentName);
            }
            return instance;
        }

        /// <summary>
        /// Throws if null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The specified value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="value"/> is null or empty.
        /// </exception>
        public static string ThrowIfEmpty(this string value, string argumentName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("The specified string is null or empty", argumentName);
            }
            return value;
        }

        #endregion

        #region String

        /// <summary>
        /// Determines whether the specified text is alpha.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// 	<c>true</c> if the specified text is alpha; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlpha(this string text)
        {
            return new Regex("^[a-zA-Z]").IsMatch(text);
        }

        /// <summary>
        /// Determines whether the specified text is numeric.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// 	<c>true</c> if the specified text is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string text)
        {
            return new Regex("^[0-9]").IsMatch(text);
        }

        /// <summary>
        /// Replaces clear line feeds with break lines
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddBreakLines(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value.Replace("\n", "<br>");
        }
        #endregion

        #region Date 

        /// <summary>
        /// Converts a datetime into a more friendly outlook style string 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string OutlookStyle(this DateTime? value)
        {
            if (value != null)
            {
                return value.Value.OutlookStyle();
            }
            return string.Empty;
        }

        /// <summary>
        /// Converts a datetime into a more friendly outlook style string 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string OutlookStyle(this DateTime value)
        {
            var timeText = value.ToString("hh:mm tt");
            if (value.Date == DateTime.Now.Date)
            {
                //Today keyword
                return string.Format("Today, {0}", timeText);
            }
            else if (value.Date == DateTime.Now.AddDays(-1).Date)
            {
                //Yesterday keyword
                return string.Format("Yesterday, {0}", timeText);
            }
            else
            {
                return value.ToString("dd/MM/yy, hh:mm tt");
            }
        }

        #endregion

        #region Miscellaneous

        /// <summary>
        /// Enumerates the enumerable, executing the action against each action along the way.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="action">The action.</param>
        /// <returns>The value.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> value, Action<T> action)
        {
            foreach (var t in value)
            {
                action(t);
            }
            return value;
        }

        /// <summary>
        /// Yields the specified value as an enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the value to be yielded.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>An enumeration of the specified value.</returns>
        public static IEnumerable<T> Yield<T>(this T value)
        {
            yield return value;
        }

        /// <summary>
        /// Converts a string to an enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to convert to.</typeparam>
        /// <param name="value">The value to be converted to an enumeration.</param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct, IConvertible
        {
            value.ThrowIfEmpty("value");
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type.");
            }
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        /// <summary>
        /// Descriptions the specified enum value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns></returns>
        public static string Description(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var field = enumType.GetField(enumValue.ToString());
            var attributes =
                field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length == 0
                ? enumValue.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }

        /// <summary>
        /// If value is null, return default, otherwise return the evaluation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="evaluation">The evaluation.</param>
        /// <returns></returns>
        public static U Attempt<T, U>(this T value, Func<T, U> evaluation)
        {
            if (null == value) return default(U);
            return evaluation(value);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>(comparer);
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        #endregion
    }
}
