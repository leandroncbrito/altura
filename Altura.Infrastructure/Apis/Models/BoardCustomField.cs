namespace Altura.Infrastructure.Apis.Models
{
    public class BoardCustomField
    {
        public string ModelType { get; } = "board";
        public string Name { get; private set; }
        public string Type { get; private set; }

        public BoardCustomField(BoardCustomFieldName name, BoardCustomFieldType type)
        {
            Name = name.ToString();
            Type = type.ToString();
        }
    }

    public record BoardCustomFieldType(string type)
    {
        public static BoardCustomFieldType Text { get; } = new("text");
        public static BoardCustomFieldType Number { get; } = new("number");
        public static BoardCustomFieldType Date { get; } = new("date");
        public static BoardCustomFieldType Checkbox { get; } = new("checkbox");
        public override string ToString() => type;
    }

    public record BoardCustomFieldName(string name)
    {
        public static BoardCustomFieldName Id { get; } = new("Id");
        public static BoardCustomFieldName TenderId { get; } = new("TenderId");
        public static BoardCustomFieldName LotNumber { get; } = new("LotNumber");
        public static BoardCustomFieldName ExpirationDate { get; } = new("ExpirationDate");
        public static BoardCustomFieldName HasDocuments { get; } = new("HasDocuments");
        public static BoardCustomFieldName Location { get; } = new("Location");
        public static BoardCustomFieldName PublicationDate { get; } = new("PublicationDate");
        public static BoardCustomFieldName Status { get; } = new("Status");
        public static BoardCustomFieldName Currency { get; } = new("Currency");
        public static BoardCustomFieldName Value { get; } = new("Value");

        public override string ToString() => name;
    }
}