using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Altura
{
    public class TenderMap : ClassMap<Tender>
    {
        public TenderMap()
        {
            Map(m => m.Id).Name("Id").TypeConverter<GuidConverter>();
            Map(m => m.TenderId).Name("TenderId").TypeConverter<GuidConverter>();
            Map(m => m.LotNumber).Name("LotNumber");
            Map(m => m.Deadline).Name("Deadline").TypeConverter<DateTimeOffsetConverter>();
            Map(m => m.Name).Name("Name");
            Map(m => m.TenderName).Name("TenderName");
            Map(m => m.ExpirationDate).Name("ExpirationDate").TypeConverter<DateTimeOffsetConverter>();
            Map(m => m.HasDocuments).Name("HasDocuments").TypeConverter<BooleanConverter>();
            Map(m => m.Location).Name("Location");
            Map(m => m.PublicationDate).Name("PublicationDate").TypeConverter<DateTimeOffsetConverter>();
            Map(m => m.Status).Name("Status");
            Map(m => m.Currency).Name("Currency");
            Map(m => m.Value).Name("Value").TypeConverter<CustomDecimalConverter>();
        }
    }

    public class CustomDecimalConverter : DecimalConverter
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
}
