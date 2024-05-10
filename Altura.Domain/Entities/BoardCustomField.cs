namespace Altura.Domain
{
    public class BoardCustomField
    {
        public string ModelType { get; } = "board";
        public string Name { get; private set; }
        public string Type { get; private set; }

        public BoardCustomField(string name, BoardCustomFieldType type)
        {
            Name = name;
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
}
