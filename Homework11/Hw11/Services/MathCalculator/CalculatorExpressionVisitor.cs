using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator
{

    [ExcludeFromCodeCoverage]
    public class CalculatorExpressionVisitor
    {
        private readonly Expression root;
        private readonly Dictionary<Expression, Expression[]> executeBefore = new();

        public CalculatorExpressionVisitor(Expression root) => this.root = root;

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

        public async Task<Dictionary<Expression, Expression[]>> GetExecuteBeforeDictAsync()
        {
            await VisitAsync((dynamic)root);

            return executeBefore;
        }
    }
}
