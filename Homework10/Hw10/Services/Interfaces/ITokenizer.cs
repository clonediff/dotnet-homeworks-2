using Hw10.Dto;

namespace Hw10.Services.Interfaces
{
    public interface ITokenizer
    {
        public TokenizerResult GetTokens(string expression);
    }
}
