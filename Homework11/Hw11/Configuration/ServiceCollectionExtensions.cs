using Hw11.Services.MathCalculator;

namespace Hw11.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        services.AddSingleton<IParser, Parser>();
        services.AddSingleton<ITokenizer, Tokenizer>();
        services.AddSingleton<ICalculator, Calculator>();
        return services.AddSingleton<IMathCalculatorService, MathCalculatorService>();
    }
}