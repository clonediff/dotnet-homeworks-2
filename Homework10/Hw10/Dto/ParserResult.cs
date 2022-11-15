using System.Linq.Expressions;

namespace Hw10.Dto
{
    public class ParserResult : ResultDto<Expression>
    {
        public ParserResult(string errorMsg)
            : base(false, default, errorMsg)
        { }

        public ParserResult(Expression data)
            : base(true, data, default)
        { }
    }
}
