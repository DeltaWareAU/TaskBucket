using System;

namespace TaskBucket.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToHumanTimeString(this TimeSpan span, int significantDigits = 3)
        {
            var format = "G" + significantDigits;
            return span.TotalMilliseconds < 1000 ? span.TotalMilliseconds.ToString(format) + " Milliseconds"
                : span.TotalSeconds < 60 ? span.TotalSeconds.ToString(format) + " Seconds"
                : span.TotalMinutes < 60 ? span.TotalMinutes.ToString(format) + " Minutes"
                : span.TotalHours < 24 ? span.TotalHours.ToString(format) + " Hours"
                : span.TotalDays.ToString(format) + " Days";
        }
    }
}