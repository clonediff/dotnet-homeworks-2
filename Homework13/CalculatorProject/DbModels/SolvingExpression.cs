using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CalculatorProject.DbModels;

[ExcludeFromCodeCoverage]
public class SolvingExpression
{
	public int SolvingExpressionId { get; set; }
		
	[Required] 
	public string Expression { get; set; } = null!;

	[Required] 
	public double Result { get; set; }
}