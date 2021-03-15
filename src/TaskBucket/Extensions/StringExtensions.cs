namespace TaskBucket.Extensions
{
    internal static class StringExtensions
    {
        public static bool EqualsAny(this string value, params string[] checkValues)
        {
            foreach(string checkValue in checkValues)
            {
                if(value.Equals(checkValue))
                {
                    continue;
                }

                return false;
            }

            return true;
        }
    }
}
