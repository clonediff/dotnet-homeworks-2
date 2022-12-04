using CalculatorProject.Dto;
using System.Linq.Expressions;

namespace CalculatorProject.Services.Interfaces
{
    public interface ICalculator
    {
        public Task<CalculationMathExpressionResultDto> CalculateAsync(Expression expression);
    }
}
