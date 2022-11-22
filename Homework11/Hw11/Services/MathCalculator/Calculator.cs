using Hw11.ErrorMessages;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator
{
    public class Calculator : ICalculator
    {
        readonly static Dictionary<ExpressionType, Func<double, double, double>> operationsInfo = new()
        {
            [ExpressionType.Add] = (x, y) => x + y,
            [ExpressionType.Subtract] = (x, y) => x - y,
            [ExpressionType.Multiply] = (x, y) => x * y,
            [ExpressionType.Divide] = (x, y) => x / y,
        };

        [ExcludeFromCodeCoverage]
        public async Task<double> CalculateAsync(Expression expression)
        {
            var executeBefore = await new CalculatorExpressionVisitor(expression)
                .GetExecuteBeforeDictAsync();
            var lazy = new Dictionary<Expression, Lazy<Task<double>>>();
            var mainExpression = expression;

            foreach (var (root, before) in executeBefore)
                lazy[root] = new Lazy<Task<double>>(async () =>
                {
                    if (before.Any())
                        await Task.WhenAll(before.Select(b => lazy[b].Value));

                    return await Eval(lazy, (dynamic)root);
                });

            return await lazy[mainExpression].Value;
        }

        async Task<double> Eval(Dictionary<Expression, Lazy<Task<double>>> lazy, BinaryExpression be)
        {
            await Task.Delay(1000);
            var left = await lazy[be.Left].Value;
            var right = await lazy[be.Right].Value;
            if (right == 0 && be.NodeType == ExpressionType.Divide)
                throw new DivideByZeroException(MathErrorMessager.DivisionByZero);
            var value = operationsInfo[be.NodeType](left, right);
            return value;
        }

        async Task<double> Eval(Dictionary<Expression, Lazy<Task<double>>> lazy, ConstantExpression constExpr)
            => (double)constExpr.Value!;

        [ExcludeFromCodeCoverage]
        private class CalculatorExpressionVisitor
        {
            private readonly Expression root;
            private readonly Dictionary<Expression, Expression[]> executeBefore = new();

            public CalculatorExpressionVisitor(Expression root) => this.root = root;

            public async Task VisitAsync(BinaryExpression binary)
            {
                await VisitAsync((dynamic)binary.Left);
                await VisitAsync((dynamic)binary.Right);
                executeBefore[binary] = new[] { binary.Left, binary.Right };
            }

            public async Task VisitAsync(ConstantExpression expression)
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
}
