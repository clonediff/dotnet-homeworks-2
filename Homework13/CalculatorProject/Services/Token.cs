namespace CalculatorProject.Services
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
    }
}
