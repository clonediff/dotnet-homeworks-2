using CalculatorProject.Dto;
using CalculatorProject.ErrorMessages;
using CalculatorProject.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;

namespace CalculatorProject.Services
{
    public class Parser : IParser
    {
        ITokenizer tokenizer;

        public Parser(ITokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        readonly static Dictionary<string, (int priority, Func<Expression, Expression, Expression> expression)> operationInfo = new()
        {
            ["+"] = (1, Expression.Add),
            ["-"] = (1, Expression.Subtract),
            ["*"] = (2, Expression.Multiply),
            ["/"] = (2, Expression.Divide)
        };

        public ParserResult ParseExpression(string? expression)
        {
            if (string.IsNullOrEmpty(expression))
                return new ParserResult(MathErrorMessager.EmptyString);
            var tokens = tokenizer.GetTokens(expression);
            if (!tokens.IsSuccess)
                return new ParserResult(tokens.ErrorMessage);
            if (!CheckTokens(tokens.Result, out var message))
                return new ParserResult(message);

            return CreateExpression(tokens.Result);
        }

        private bool TryParseDouble(string arg, out double result)
            => double.TryParse(arg,
                        NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out result);

        private bool CheckParanthesis(List<Token> tokens)
        {
            var stack = new Stack<bool>();
            foreach (var token in tokens)
                if (token.Type == TokenType.Paranthesis)
                    if (token.Value == "(")
                        stack.Push(true);
                    else
                    {
                        if (stack.Count == 0) return false;
                        stack.Pop();
                    }
            return stack.Count == 0;
        }

        [ExcludeFromCodeCoverage]
        bool CheckTokens(List<Token> tokens, out string errorMessage)
        {
            errorMessage = default!;

            if (tokens.Count > 0 && tokens[0].Type == TokenType.Operation)
            {
                errorMessage = MathErrorMessager.StartingWithOperation;
                return false;
            }
            if (tokens.Count > 0 && tokens[^1].Type == TokenType.Operation)
            {
                errorMessage = MathErrorMessager.EndingWithOperation;
                return false;
            }
            if (!CheckParanthesis(tokens))
            {
                errorMessage = MathErrorMessager.IncorrectBracketsNumber;
                return false;
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TokenType.Number && !TryParseDouble(tokens[i].Value, out var _))
                {
                    errorMessage = MathErrorMessager.NotNumberMessage(tokens[i].Value);
                    return false;
                }
                if (i > 0 && tokens[i].Value == ")" && tokens[i - 1].Type == TokenType.Operation)
                {
                    errorMessage = MathErrorMessager.OperationBeforeParenthesisMessage(tokens[i - 1].Value);
                    return false;
                }
                if (i > 0 && tokens[i - 1].Value == "(" && tokens[i].Type == TokenType.Operation)
                {
                    errorMessage = MathErrorMessager.InvalidOperatorAfterParenthesisMessage(tokens[i].Value);
                    return false;
                }
                if (i > 0 && tokens[i - 1].Type == TokenType.Operation && tokens[i].Type == TokenType.Operation)
                {
                    errorMessage = MathErrorMessager.TwoOperationInRowMessage(tokens[i - 1].Value, tokens[i].Value);
                    return false;
                }
                if (i > 0 && tokens[i - 1].Type == TokenType.Number && tokens[i].Type == TokenType.Number)
                {
                    errorMessage = MathErrorMessager.TwoNumberInRowMessage(tokens[i - 1].Value, tokens[i].Value);
                    return false;
                }
            }
            return true;
        }

        private void SetLastOperation(Stack<Expression> expressionsStack, Stack<string> operationsStack)
        {
            var rightExpr = expressionsStack.Pop();
            var leftExpr = expressionsStack.Pop();
            var operation = operationsStack.Pop();
            var mergedExpr = operationInfo[operation].expression(leftExpr, rightExpr);
            expressionsStack.Push(mergedExpr);
        }

        private ParserResult CreateExpression(List<Token> tokens)
        {
            var expressionsStack = new Stack<Expression>();
            var operationsStack = new Stack<string>();

            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Number)
                {
                    TryParseDouble(token.Value, out var tokenValue);
                    expressionsStack.Push(Expression.Constant(tokenValue));
                }
                if (token.Type == TokenType.Operation)
                {
                    if (operationsStack.Count != 0 && operationsStack.Peek() != "(" &&
                        operationInfo[operationsStack.Peek()].priority >= operationInfo[token.Value].priority)
                            SetLastOperation(expressionsStack, operationsStack);
                    operationsStack.Push(token.Value);
                }
                if (token.Type == TokenType.Paranthesis)
                {
                    if (token.Value == "(")
                        operationsStack.Push(token.Value);
                    else
                    {
                        while (operationsStack.Peek() != "(")
                            SetLastOperation(expressionsStack, operationsStack);
                        operationsStack.Pop();
                    }
                }
            }

            while (operationsStack.Count != 0)
                SetLastOperation(expressionsStack, operationsStack);

            return new ParserResult(expressionsStack.Pop());
        }
    }
}
