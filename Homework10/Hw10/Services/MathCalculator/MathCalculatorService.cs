using Hw10.Dto;
using Hw10.Services.Interfaces;

namespace Hw10.Services.MathCalculator;

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