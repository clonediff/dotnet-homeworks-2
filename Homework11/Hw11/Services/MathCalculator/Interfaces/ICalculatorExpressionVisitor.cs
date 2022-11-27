using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator.Interfaces
{
    public interface ICalculatorExpressionVisitor
    {
        Task<Dictionary<Expression, Expression[]>> GetExecuteBeforeDictAsync(Expression root);
    }
}
