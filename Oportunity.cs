namespace Altura
{
    /*
     * : Id, TenderId, LotNumber, Deadline,
         Name, TenderName, ExpirationDate, HasDocuments, Location, PublicationDate, Status,
         Currency, Value
     */
    public class Oportunity
    {
        public required Guid Id { get; set; }
        public required Guid TenderId { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public DateTime? Deadline { get; set; }
        public required string Name { get; set; }
        public required string TenderName { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool HasDocuments { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        // TODO: enum?
        public byte Status { get; set;}
        public string Currency { get; set; } = string.Empty;
        public decimal? Value { get; set; }
    }
}
