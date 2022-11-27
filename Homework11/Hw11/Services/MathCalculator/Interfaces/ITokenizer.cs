namespace Hw11.Services.MathCalculator
{
    public interface ITokenizer
    {
        public List<Token> GetTokens(string expression);
    }
}
