using CalculatorProject.DbModels;
using CalculatorProject.Services;
using CalculatorProject.Services.CachedCalculator;
using CalculatorProject.Services.Interfaces;
using CalculatorProject.Services.MathCalculator;
using Hw9.Services.MathCalculator;

namespace CalculatorProject.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        services
            .AddSingleton<IParser, Parser>();
        services
            .AddSingleton<ITokenizer, Tokenizer>();
        services
            .AddSingleton<ICalculator, Calculator>();
        return services.AddTransient<MathCalculatorService>();
    }
    
    public static IServiceCollection AddCachedMathCalculator(this IServiceCollection services)
    {
        return services.AddScoped<IMathCalculatorService>(s =>
            new MathCachedCalculatorService(
                s.GetRequiredService<MathCalculatorService>()));
    }
}