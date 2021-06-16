using System;
using System.Linq;

namespace SnackisDB
{
    public static class ExtensionMethods
    {
        public static string DaysAgo(this DateTime dt)
        {

            string hours = dt.Hour.ToString();
            string minutes = dt.Minute.ToString();
            var now = DateTime.Now;
            var elasped = now.Subtract(dt);
            double daysAgo = elasped.TotalDays;

            var numbersToAddZeroTo = Enumerable.Range(0, 10);
            if (numbersToAddZeroTo.Contains(dt.Hour))
            {
                hours = "0" + hours;
            }
            if (numbersToAddZeroTo.Contains(dt.Minute))
            {
                minutes = "0" + minutes;
            }

            if (daysAgo <= 1 && now.ToShortDateString() == dt.ToShortDateString())
            {
                return $"idag {hours}:{minutes}";
            }
            else if (daysAgo <= 1)
            {
                return $"igår {hours}:{minutes}";
            }
            else if (daysAgo < 8)
            {
                return $"för {Math.Round(daysAgo, 0)} dagar sedan kl {hours}:{minutes}";
            }
            else
            {
                return $"{dt.ToShortTimeString()} {hours}:{minutes}";
            }

        }
    }
}
