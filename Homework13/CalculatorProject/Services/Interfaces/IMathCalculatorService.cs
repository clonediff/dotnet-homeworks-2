using CalculatorProject.Dto;

namespace CalculatorProject.Services.Interfaces;

public interface IMathCalculatorService
{
    public Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression);
}