// ReSharper disable once CheckNamespace
namespace System
{
    internal static class TimeSpanExtensions
    {
        public static string ToHumanTimeString(this TimeSpan? timeSpan, int significantDigits = 3)
        {
            if (timeSpan == null)
            {
                return "NULL";
            }

            var format = "G" + significantDigits;

            if (timeSpan.Value.TotalNanoseconds < 1000)
            {
                return timeSpan.Value.TotalNanoseconds.ToString(format) + "ns";
            }

            if (timeSpan.Value.TotalMicroseconds < 1000)
            {
                return timeSpan.Value.TotalMicroseconds.ToString(format) + "µs";
            }

            if (timeSpan.Value.TotalMilliseconds < 1000)
            {
                return timeSpan.Value.TotalMilliseconds.ToString(format) + "ms";
            }

            if (timeSpan.Value.TotalSeconds < 60)
            {
                return timeSpan.Value.TotalSeconds.ToString(format) + "s";
            }

            if (timeSpan.Value.TotalMinutes < 60)
            {
                return timeSpan.Value.TotalMinutes.ToString(format) + "min";
            }

            return timeSpan.Value.TotalHours.ToString(format) + "h";
        }
    }
}