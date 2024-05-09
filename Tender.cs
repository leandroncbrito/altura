namespace Altura
{
    public class Tender
    {
        public required Guid Id { get; set; }
        public required Guid TenderId { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public DateTimeOffset? Deadline { get; set; }
        public required string Name { get; set; }
        public required string TenderName { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
        public bool HasDocuments { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTimeOffset? PublicationDate { get; set; }        
        public byte Status { get; set;}
        public string Currency { get; set; } = string.Empty;
        public decimal? Value { get; set; }
    }
}
