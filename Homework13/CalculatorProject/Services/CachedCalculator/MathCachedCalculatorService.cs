using CalculatorProject.DbModels;
using CalculatorProject.Dto;
using CalculatorProject.Services.Interfaces;

namespace CalculatorProject.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	static List<SolvingExpression> SolvingExpressions { get; } = new();
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(IMathCalculatorService simpleCalculator)
	{
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var cachedResult = SolvingExpressions
			.FirstOrDefault(x => x.Expression == expression);

		if (cachedResult is not null)
			return new CalculationMathExpressionResultDto(cachedResult.Result);

		var result = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		if (result.IsSuccess)
			SolvingExpressions.Add(new SolvingExpression { Expression = expression, Result = result.Result });

		return result;
	}
}