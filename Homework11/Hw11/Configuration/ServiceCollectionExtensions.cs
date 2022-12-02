using Hw11.Services.MathCalculator;
using Hw11.Services.MathCalculator.Interfaces;

namespace Hw11.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        services.AddSingleton<IParser, Parser>();
        services.AddSingleton<ITokenizer, Tokenizer>();
        services.AddSingleton<ICalculator, Calculator>();
        services.AddSingleton<ICalculatorExpressionVisitor, CalculatorExpressionVisitor>();
        return services.AddSingleton<IMathCalculatorService, MathCalculatorService>();
    }
}