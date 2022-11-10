using Hw9.Dto;

namespace Hw9.Services.MathCalculator
{
    public interface ITokenizer
    {
        public TokenizerResult GetTokens(string expression);
    }
}
