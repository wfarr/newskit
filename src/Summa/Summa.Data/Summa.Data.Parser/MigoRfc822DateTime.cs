/***************************************************************************
 *  Rfc822DateTime.cs
 *
 *  Copyright (C) 2006 Michael C. Urbanski
 *  Written by Mike Urbanski <michael.c.urbanski@gmail.com>
 ****************************************************************************/

/*  THIS FILE IS LICENSED UNDER THE MIT LICENSE AS OUTLINED IMMEDIATELY BELOW:
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a
 *  copy of this software and associated documentation files (the "Software"),  
 *  to deal in the Software without restriction, including without limitation  
 *  the rights to use, copy, modify, merge, publish, distribute, sublicense,  
 *  and/or sell copies of the Software, and to permit persons to whom the  
 *  Software is furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 *  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
 *  DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Text.RegularExpressions;

namespace Migo.Syndication
{
    public static class Rfc822DateTime
    {
        private const string monthsStr = 
            "Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|" +
            "January|February|March|April|May|June|July|August|" +
            "September|October|November|December";
        
        private const string daysOfWeek = 
            "Mon|Tue|Wed|Thu|Fri|Sat|Sun|" +
            "Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday";
        
        private const string rfc822DTExp =
            @"^(?<dayofweek>(" + daysOfWeek + "), )?" +
            @"(?<day>\d\d?) " +
            @"(?<month>" + monthsStr + ") " +
            @"(?<year>\d\d(\d\d)?) " +
            @"(?<hours>[0-2]?\d):(?<minutes>[0-5]\d)(:(?<seconds>[0-5]\d))?" +
            @"( (?<timezone>[A-I]|[K-Z]|GMT|UT|EST|EDT|CST|CDT|MST|MDT|PST|PDT|([+-]\d\d\d\d))$)?";

        private static readonly string[] months;
        private static readonly Regex rfc822DTRegex;

        static Rfc822DateTime()
        {
            months = monthsStr.Split ('|');
            rfc822DTRegex = new Regex (rfc822DTExp, 
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static DateTime Parse (string dateTime)
        {
            if (dateTime == null) {
                throw new ArgumentNullException ("dateTime");
            }

            Match m = rfc822DTRegex.Match (dateTime);

            if (m.Success) {
                GroupCollection groups = m.Groups;
				DateTime ret;
                int day = Convert.ToInt32 (groups ["day"].Value);
                int month = MonthToInt32 (groups ["month"].Value);
                int year = Convert.ToInt32 (groups ["year"].Value);

                int hours = Convert.ToInt32 (groups ["hours"].Value);
                int minutes = Convert.ToInt32 (groups ["minutes"].Value);

                int seconds = 0;
                string secondsStr = groups ["seconds"].Value;
                string timeZone = groups ["timezone"].Value;

                if (secondsStr != String.Empty) {
                    seconds = Convert.ToInt32 (secondsStr);
                }

                if (year < 100) {
                    int curYear = DateTime.Now.Year;
                    year = curYear - (curYear % 100) + year;
                }
				
				ret = new DateTime (year, month, day, hours, minutes, seconds);
                                
                if (timeZone != String.Empty) {
                    ret += ParseGmtOffset (timeZone);
                }
				
                return ret.ToLocalTime ();
            }

            throw new FormatException ("'dateTime' does not represent a valid RFC 822 date-time");
        }

        public static bool TryParse (string dateTime, out DateTime result)
        {
            bool ret = false;
            result = DateTime.MinValue;
            
            try {
                result = Parse (dateTime);
                ret = true;
            } catch {}
                
            return ret;
        }
        
        private static int MonthToInt32 (string month)
        {
            int i = 1;

            foreach (string s in months) {
                if (month == s) {
                    break;
                }
                
                if (++i % 13 == 0) {
                    i = 1;
                }
            }

            return i;
        }

        private static TimeSpan ParseGmtOffset (string offset)
        {
            int offsetHours = 0;
            int offsetMinutes = 0;

            if (offset.Length == 5) {
                offsetHours = Convert.ToInt32 (offset.Substring (1,2));
                offsetMinutes = Convert.ToInt32 (offset.Substring (3,2));

                if (offset [0] == '-') {
                    offsetHours *= -1;
                    offsetMinutes *= -1;
                }
            } else {
                switch (offset)
                {
                    case "GMT": case "UT": break;						
                    case "EDT": offsetHours = -4; break;
                    case "EST": case "CDT": offsetHours = -5; break;
                    case "CST": case "MDT": offsetHours = -6; break;
                    case "MST": case "PDT": offsetHours = -7; break;
                    case "PST": offsetHours = -8; break;
					
                    case "Z": offsetHours = 0; break;

                    case "A": offsetHours = -1; break;
                    case "B": offsetHours = -2; break;
                    case "C": offsetHours = -3; break;
                    case "D": offsetHours = -4; break;
                    case "E": offsetHours = -5; break;
                    case "F": offsetHours = -6; break;
                    case "G": offsetHours = -7; break;
                    case "H": offsetHours = -8; break;
                    case "I": offsetHours = -9; break;
                    
                    // Q.  Why was 'J' left out of Z-Time?
                    // A.  http://www.maybeck.com/ztime/  
                    
                    // That's what I like about this job, you learn stuff.
                    
                    case "K": offsetHours = -10; break;
                    case "L": offsetHours = -11; break;
                    case "M": offsetHours = -12; break;				
                    case "N": offsetHours = 1; break;
                    case "O": offsetHours = 2; break;
                    case "P": offsetHours = 3; break;
                    case "Q": offsetHours = 4; break;
                    case "R": offsetHours = 5; break;
                    case "S": offsetHours = 6; break;
                    case "T": offsetHours = 7; break;
                    case "U": offsetHours = 8; break;
                    case "V": offsetHours = 9; break;
                    case "W": offsetHours = 10; break;
                    case "X": offsetHours = 11; break;
                    case "Y": offsetHours = 12; break;
                }   
            }

            return TimeSpan.FromTicks (
                (offsetHours * TimeSpan.TicksPerHour) +
                (offsetMinutes * TimeSpan.TicksPerMinute)
            );
        }
    }
}
