using CalculatorProject.Dto;
using CalculatorProject.Services.Interfaces;

namespace CalculatorProject.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    IParser parser;
    ICalculator calculator;

    public MathCalculatorService(IParser parser, ICalculator calculator)
    {
        this.parser = parser;
        this.calculator = calculator;
    }

    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var parsedExpression = parser.ParseExpression(expression);
        if (!parsedExpression.IsSuccess)
            return new CalculationMathExpressionResultDto(parsedExpression.ErrorMessage);

        return await calculator.CalculateAsync(parsedExpression.Result);
    }
}