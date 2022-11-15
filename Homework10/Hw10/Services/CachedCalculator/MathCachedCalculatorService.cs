using Hw10.DbModels;
using Hw10.Dto;
using Hw10.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly ApplicationContext _dbContext;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
	{
		_dbContext = dbContext;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var cachedResult = await _dbContext.SolvingExpressions
			.FirstOrDefaultAsync(x => x.Expression == expression);

		if (cachedResult is not null)
			return new CalculationMathExpressionResultDto(cachedResult.Result);

		var result = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		if (result.IsSuccess)
		{
			_dbContext.SolvingExpressions.Add(new SolvingExpression { Expression = expression, Result = result.Result });
			await _dbContext.SaveChangesAsync();
		}

		return result;
	}
}