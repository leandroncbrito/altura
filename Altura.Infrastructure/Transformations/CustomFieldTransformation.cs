using TrelloDotNet.Model;

namespace Altura.Infrastructure.Transformations
{
    public static class CustomFieldTransformation
    {
        public static string? ConvertToString(this CustomField customField, object value)
        {
            if (customField == null || value == null)
            {
                return null;
            }

            return customField.Type switch
            {
                CustomFieldType.Text => value.ToString(),
                CustomFieldType.Number => value.ToString(),
                CustomFieldType.Checkbox => ((bool)value).ToString().ToLowerInvariant(),
                CustomFieldType.Date => ((DateTimeOffset)value).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                _ => null,
            };
        }
    }
}
