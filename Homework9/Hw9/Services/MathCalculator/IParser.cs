using Hw9.Dto;
using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator
{
    public interface IParser
    {
        public ResultDto<Expression> ParseExpression(string? expression);
    }
}
