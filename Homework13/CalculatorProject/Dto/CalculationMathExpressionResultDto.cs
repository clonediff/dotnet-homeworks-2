using System.Runtime.CompilerServices;

namespace CalculatorProject.Dto;

public class CalculationMathExpressionResultDto : ResultDto<double>
{ 
    public CalculationMathExpressionResultDto(string errorMsg)
        : base(false, default, errorMsg)
    {
    }

    public CalculationMathExpressionResultDto(double value)
        : base(true, value, default)
    {
    }

    public CalculationMathExpressionResultDto() { }
}