using Altura.Domain;
using Altura.Infrastructure.Interfaces;
using Altura.Infrastructure.Mapping;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Altura.Infrastructure.Readers
{
    public class TenderParser : ITenderParser
    {
        public IEnumerable<Tender> ParseTenders()
        {
            var models = new List<Tender>();

            using (var reader = new StreamReader("assignment-opportunities-v1.csv"))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                while (csv.Read())
                {
                    csv.Context.RegisterClassMap<TenderMap>();

                    models.Add(csv.GetRecord<Tender>());
                }
            }

            return models;
        }
    }
}
