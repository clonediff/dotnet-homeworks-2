using Hw9.Services.MathCalculator;

namespace Hw9.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        services
            .AddSingleton<IMathCalculatorService, MathCalculatorService>();
        services
            .AddSingleton<IParser, Parser>();
        services
            .AddSingleton<ITokenizer, Tokenizer>();
        return services
            .AddSingleton<ICalculator, Calculator>();
    }
}