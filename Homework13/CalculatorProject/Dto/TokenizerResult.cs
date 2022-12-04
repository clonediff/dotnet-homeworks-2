using CalculatorProject.Services;

namespace CalculatorProject.Dto
{
    public class TokenizerResult : ResultDto<List<Token>>
    {
        public TokenizerResult(string errorMsg) 
            : base(false, default, errorMsg) { }

        public TokenizerResult(List<Token> list)
            : base(true, list, default) { }
    }
}
