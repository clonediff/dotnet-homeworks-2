namespace Hw9.Dto
{
    public enum TokenType
    {
        Number,
        Operation,
        Paranthesis
    }

    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public override string ToString()
        {
            return $"{Type}: {Value}";
        }
    }
}
