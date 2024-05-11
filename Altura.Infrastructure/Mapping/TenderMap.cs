using Altura.Domain.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Altura.Infrastructure.Mapping
{
    public class TenderMap : ClassMap<Tender>
    {
        public TenderMap()
        {
            Map(m => m.Id).Name("Id").TypeConverter<GuidConverter>();
            Map(m => m.TenderId).Name("TenderId").TypeConverter<GuidConverter>();
            Map(m => m.LotNumber).Name("LotNumber");
            Map(m => m.Deadline).Name("Deadline").TypeConverter<CustomDateTimeOffsetConverter>();
            Map(m => m.Name).Name("Name");
            Map(m => m.TenderName).Name("TenderName");
            Map(m => m.ExpirationDate).Name("ExpirationDate").TypeConverter<CustomDateTimeOffsetConverter>();
            Map(m => m.HasDocuments).Name("HasDocuments").TypeConverter<BooleanConverter>();
            Map(m => m.Location).Name("Location");
            Map(m => m.PublicationDate).Name("PublicationDate").TypeConverter<CustomDateTimeOffsetConverter>();
            Map(m => m.Status).Name("Status");
            Map(m => m.Currency).Name("Currency");
            Map(m => m.Value).Name("Value").TypeConverter<CustomDecimalConverter>();
        }
    }

    internal class CustomDecimalConverter : DecimalConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (decimal.TryParse(text, out var result))
            {
                return result;
            }

            return decimal.Zero;
        }
    }

    internal class CustomDateTimeOffsetConverter : DateTimeOffsetConverter
    {
        public override object? ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (DateTimeOffset.TryParse(text, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
