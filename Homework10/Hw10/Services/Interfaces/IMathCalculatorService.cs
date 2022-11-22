using Hw10.Dto;

namespace Hw10.Services.Interfaces;

public interface IMathCalculatorService
{
    public Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression);
}