using CalculatorProject.Dto;

namespace CalculatorProject.Services.Interfaces
{
    public interface ITokenizer
    {
        public TokenizerResult GetTokens(string expression);
    }
}
