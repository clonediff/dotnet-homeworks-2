using Hw10.Services;

namespace Hw10.Dto
{
    public class TokenizerResult : ResultDto<List<Token>>
    {
        public TokenizerResult(string errorMsg) 
            : base(false, default, errorMsg) { }

        public TokenizerResult(List<Token> list)
            : base(true, list, default) { }
    }
}
