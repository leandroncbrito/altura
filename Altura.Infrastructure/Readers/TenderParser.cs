using Altura.Domain;
using Altura.Infrastructure.Interfaces;
using Altura.Infrastructure.Mapping;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Altura.Infrastructure.Readers
{
    public class TenderParser : ITenderParser
    {
        private readonly ILogger<TenderParser> _logger;

        public TenderParser(ILogger<TenderParser> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Tender> ParseTenders()
        {
            var tenders = new List<Tender>();

            try
            {
                using (var reader = new StreamReader("../Altura.Infrastructure/DataSource/assignment-opportunities-v1.csv"))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    while (csv.Read())
                    {
                        csv.Context.RegisterClassMap<TenderMap>();

                        tenders.Add(csv.GetRecord<Tender>());
                    }
                }
            }
            catch (FileNotFoundException)
            {
                _logger.LogError("Error: The specified file does not exist.");
                throw;
            }
            catch (CsvHelperException ex)
            {                
               _logger.LogError("Error while parsing CSV data: " + ex.Message, ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred: " + ex.Message, ex);
                throw;
            }

            return tenders;
        }
    }
}
