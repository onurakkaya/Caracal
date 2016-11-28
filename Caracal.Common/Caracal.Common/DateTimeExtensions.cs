using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Common
{
    public static class DateTimeExtensions
    {
        public static DateTime CheckForSql(this DateTime dateTime)
        {
            return dateTime.Year < 1900 ? dateTime.AddYears(dateTime.Year * 0 + 1899) : dateTime;
        }
        public static string ToSqlString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
