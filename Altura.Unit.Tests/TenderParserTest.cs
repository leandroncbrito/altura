using Altura.Domain.Entities;
using Altura.Infrastructure.Readers;
using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace Altura.Unit.Tests
{
    public class TenderParserTest
    {
        [Fact]
        public void TenderParse_Should_Read_Csv_Properly()
        {
            var mockLogger = new Mock<ILogger<TenderParser>>();

            var sut = new TenderParser(mockLogger.Object);

            var response = sut.ParseTenders("DataSource/test-opportunities.csv");

            Assert.NotNull(response);

            var tenders = response.ToList();

            Assert.Equal(10, tenders.Count);

            var firstTender = tenders.First();

            Assert.Equal(Guid.Parse("7A3DAA68-458F-4F92-A7D9-08DBBF541284"), firstTender.Id);
            Assert.Equal(Guid.Parse("6A07D6E2-6C07-4554-89CB-D8174EEE9D68"), firstTender.TenderId);
            Assert.Equal("Marktconsultatie - Gemeente Zwolle - Leveren van verlichting voor vier iconen", firstTender.Name);
            Assert.Equal("Marktconsultatie - Gemeente Zwolle - Leveren van verlichting voor vier iconen", firstTender.TenderName);
            Assert.Null(firstTender.ExpirationDate);
            Assert.True(firstTender.HasDocuments);
            Assert.Equal("Nederland", firstTender.Location);
            Assert.Equal(new DateTimeOffset(2023, 09, 09, 22, 01, 45, 442, TimeSpan.Zero), firstTender.PublicationDate);
            Assert.Equal(3, firstTender.Status);
            Assert.Equal("EUR", firstTender.Currency);
            Assert.Equal(651651, firstTender.Value);
        }
    }
}