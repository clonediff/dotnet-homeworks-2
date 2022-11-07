using Hw9.Dto;
using Hw9.ErrorMessages;
using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Hw9.Services.MathCalculator;

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
        // Для имитации длительного процесса
        await Task.Delay(1000);

        var parsedExpression = await Task.Run(() => parser.ParseExpression(expression));
        if (!parsedExpression.IsSuccess)
            return new CalculationMathExpressionResultDto(parsedExpression.ErrorMessage);

        return await calculator.CalculateAsync(parsedExpression.Result);
    }
}