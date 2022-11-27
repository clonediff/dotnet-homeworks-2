using Hw9.Services.MathCalculator;

namespace Hw9.Dto
{
    public class TokenizerResult : ResultDto<List<Token>>
    {
        public TokenizerResult(string errorMsg) 
            : base(false, default, errorMsg) { }

        public TokenizerResult(List<Token> list)
            : base(true, list, default) { }
    }
}
