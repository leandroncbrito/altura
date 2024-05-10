using TrelloDotNet.Model;

namespace Altura.Infrastructure.Transformations
{
    public static class CustomFieldTransformation
    {
        public static string? ConvertToString(this CustomField customField, object value)
        {
            if (value == null)
            {
                return null;
            }

            switch (customField.Type.ToString())
            {
                case "Text":
                case "Number":
                    return value.ToString();
                case "Checkbox":
                    return value.ToString().ToLowerInvariant();
                case "Date":                    
                    return ((DateTimeOffset)value).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                default: 
                    return null;
            }
        }
    }
}
