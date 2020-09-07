// --------------------------------------------------------------------------------
/*  Copyright © 2020, Yasgar Technology Group, Inc.
    Any unauthorized review, use, disclosure or distribution is prohibited.

    Purpose: DateTime extension methods

    Description: 

*/
// --------------------------------------------------------------------------------

using System;

namespace YTG.CSVtoGPX.Code
{
    public static class DateExtensions
    {

        /// <summary>
        /// Retrieve a DateTime string to the milliseconds for time stamps etc.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateTimeTimeStampString(this DateTime value)
        {
            if (value == null || value == DateTime.MinValue)
            { return "1111111111111111111"; }
            if (value == DateTime.MaxValue)
            { return "9999999999999999999"; }

            string _timestamp = value.Year.ToString() + value.Month.ToString("0#") + value.Day.ToString("0#");
            _timestamp += value.Hour.ToString() + value.Minute.ToString("0#") + value.Second.ToString("0#");
            _timestamp += value.Millisecond.ToString("0000#");

            return _timestamp;

        }


    }
}
