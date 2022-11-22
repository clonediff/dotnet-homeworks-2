using Hw10.Dto;
using System.Linq.Expressions;

namespace Hw10.Services.Interfaces
{
    public interface ICalculator
    {
        public Task<CalculationMathExpressionResultDto> CalculateAsync(Expression expression);
    }
}
