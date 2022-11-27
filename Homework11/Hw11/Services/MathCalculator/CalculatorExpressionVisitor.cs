using Hw11.Services.MathCalculator.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator
{

    [ExcludeFromCodeCoverage]
    public class CalculatorExpressionVisitor : ICalculatorExpressionVisitor
    {
        private readonly Dictionary<Expression, Expression[]> executeBefore = new();

        async Task VisitAsync(BinaryExpression binary)
        {
            await VisitAsync((dynamic)binary.Left);
            await VisitAsync((dynamic)binary.Right);
            executeBefore[binary] = new[] { binary.Left, binary.Right };
        }

        async Task VisitAsync(ConstantExpression expression)
        {
            executeBefore[expression] = Array.Empty<Expression>();
        }

        public async Task<Dictionary<Expression, Expression[]>> GetExecuteBeforeDictAsync(Expression root)
        {
            await VisitAsync((dynamic)root);

            return executeBefore;
        }
    }
}
