﻿using Hw9.Dto;
using Hw9.ErrorMessages;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Hw9.Services.MathCalculator
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

        public async Task<CalculationMathExpressionResultDto> CalculateAsync(Expression expression)
        {
            var executeBefore = await Task.Run(()
                => new CalculatorExpressionVisitor(expression).GetExecuteBeforeDict());
            var lazy = new Dictionary<Expression, Lazy<Task<CalculationMathExpressionResultDto>>>();
            var mainExpression = expression;

            foreach (var (root, before) in executeBefore)
                lazy[root] = new Lazy<Task<CalculationMathExpressionResultDto>>(async () =>
                {
                    if (before.Any())
                    {
                        await Task.WhenAll(before.Select(b => lazy[b].Value));
                        await Task.Yield();
                    }

                    if (root is BinaryExpression be)
                    {
                        var left = await lazy[be.Left].Value;
                        var right = await lazy[be.Right].Value;
                        if (!left.IsSuccess)
                            return left;
                        if (!right.IsSuccess)
                            return right;
                        if (right.Result == 0 && be.NodeType == ExpressionType.Divide)
                            return new CalculationMathExpressionResultDto(MathErrorMessager.DivisionByZero);
                        var value = operationsInfo[be.NodeType](left.Result, right.Result);
                        return new CalculationMathExpressionResultDto(value);
                    }
                    else // иначе ConstantExpression
                    {
                        var constantExpr = (root as ConstantExpression)!;
                        return new CalculationMathExpressionResultDto((double)constantExpr.Value!);
                    }
                });

            return await lazy[mainExpression].Value;
        }

        private class CalculatorExpressionVisitor
        {
            private readonly Expression root;
            private readonly Dictionary<Expression, Expression[]> executeBefore = new Dictionary<Expression, Expression[]>();

            public CalculatorExpressionVisitor(Expression root) => this.root = root;

            public void Visit(Expression expression)
            {
                if (executeBefore.ContainsKey(expression))
                    return;

                Expression[] toExecute;
                if (expression is BinaryExpression binary)
                {
                    toExecute = new[] { binary.Left, binary.Right };
                    Visit(binary.Left);
                    Visit(binary.Right);
                }
                else // иначе ConstantExpression
                    toExecute = Array.Empty<Expression>();

                executeBefore[expression] = toExecute;
            }

            public Dictionary<Expression, Expression[]> GetExecuteBeforeDict()
            {
                Visit(root);

                return executeBefore;
            }
        }
    }
}
