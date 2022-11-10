using Hw9.Dto;
using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator
{
    public interface ICalculator
    {
        public Task<CalculationMathExpressionResultDto> CalculateAsync(Expression expression);
    }
}
