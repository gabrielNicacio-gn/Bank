using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplifiedBank.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetFirstDay(this DateTime date)
        => new(date.Year, date.Month, 1);

        public static DateTime GetLastDay(this DateTime date)
        => new DateTime(date.Year, date.Month, 1)
            .AddMonths(1)
            .AddDays(-1);


    }
}