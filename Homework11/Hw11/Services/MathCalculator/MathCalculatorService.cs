using Hw11.Exceptions;

namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    IParser parser;
    ICalculator calculator;
    IExceptionHandler exceptionHandler;

    public MathCalculatorService(IParser parser, ICalculator calculator, IExceptionHandler exceptionHandler)
    {
        this.parser = parser;
        this.calculator = calculator;
        this.exceptionHandler = exceptionHandler;
    }

    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        var parsedExpression = parser.ParseExpression(expression);
        return await calculator.CalculateAsync(parsedExpression);
    }
}