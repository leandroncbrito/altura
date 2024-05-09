using System.Runtime.CompilerServices;

namespace Altura
{
    public static class StringExntesions
    {
        public static bool IsEqualTo(this string str, string value)
        {
            if (str == null && value == null)
            {
                return true;
            }

            if (str == null || value == null) 
            {
                return false;
            }

            return str.Equals(value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
