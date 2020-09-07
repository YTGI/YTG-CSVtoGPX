// --------------------------------------------------------------------------------
/*  Copyright © 2020, Yasgar Technology Group, Inc.
    Any unauthorized review, use, disclosure or distribution is prohibited.

    Purpose: String Extension Methods

    Description: To augment some functions needed for strings

*/
// --------------------------------------------------------------------------------

using System;

namespace YTG.CSVtoGPX.Code
{
    public static class StringExtensions
    {

        /// <summary>
        /// Convert a string to a Decimal, return null if fails
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal? ToDecimal(this string value)
        {
            Decimal decItem;
            if (Decimal.TryParse(value, out decItem))
            { return decItem; }
            else
            { return null; }

        }

        /// <summary>
        /// Trim a string down to a particular size
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Trim(this string value, int p_MaxLength)
        {
            try
            {
                if (value != null)
                {
                    return value.Substring(0, Math.Min(p_MaxLength, value.Length));
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
