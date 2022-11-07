using Hw9.Dto;
using Hw9.ErrorMessages;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace Hw9.Services.MathCalculator
{
    public class Parser : IParser
    {
        const string tokenizerRegex = @"\s*(\(\s*\-\s*[0-9\.]+\s*\)|[0-9\.]+|\S)\s*";
        readonly static Dictionary<string, (int priority, Func<Expression, Expression, Expression> expression)> operationInfo = new()
        {
            ["+"] = (1, Expression.Add),
            ["-"] = (1, Expression.Subtract),
            ["*"] = (2, Expression.Multiply),
            ["/"] = (2, Expression.Divide)
        };

        public ResultDto<Expression> ParseExpression(string? expression)
        {
            if (string.IsNullOrEmpty(expression))
                return ResultDto<Expression>.Error(MathErrorMessager.EmptyString);
            var tokens = GetTokens(expression);
            if (!tokens.IsSuccess)
                return ResultDto<Expression>.Error(tokens.ErrorMessage);

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

        private ResultDto<List<Token>> GetTokens(string expression)
        {
            var tokens = Regex.Matches(expression, tokenizerRegex).Select(x => x.Value.Replace(" ", "")).ToArray();
            var resultTokens = new List<Token>();
            bool prevIsNegative = false;
            for (int i = 0; i < tokens.Length; i++)
            {
                if ((i == 0 || tokens[i - 1] == "(") && tokens[i] == "-")
                {
                    prevIsNegative = true;
                    continue;
                }

                var curToken = tokens[i];
                if (prevIsNegative)
                {
                    if (!TryParseDouble(curToken, out var _))
                        resultTokens.Add(new Token { Type = TokenType.Operation, Value = "-"});
                    else
                        curToken = "-" + tokens[i];
                }
                else if (Regex.Match(tokens[i], @"\(-[0-9\.]+\)").Success)
                    curToken = tokens[i].Substring(1, tokens[i].Length - 2);

                TokenType tokenType = default;
                if (curToken == "(" ||
                    curToken == ")")
                {
                    if (resultTokens.Count > 0 && curToken == ")" && resultTokens[^1].Type == TokenType.Operation)
                        return ResultDto<List<Token>>.Error(MathErrorMessager.OperationBeforeParenthesisMessage(resultTokens[^1].Value));
                    tokenType = TokenType.Paranthesis;
                }
                else if (curToken == "+" ||
                        curToken == "-" ||
                        curToken == "*" ||
                        curToken == "/")
                {
                    if (resultTokens.Count > 0 && resultTokens[^1].Value == "(")
                        return ResultDto<List<Token>>.Error(MathErrorMessager.InvalidOperatorAfterParenthesisMessage(curToken));
                    if (resultTokens.Count > 0 && resultTokens[^1].Type == TokenType.Operation)
                        return ResultDto<List<Token>>.Error(MathErrorMessager.TwoOperationInRowMessage(resultTokens[^1].Value, curToken));
                    tokenType = TokenType.Operation;
                }
                else if (Regex.Match(curToken, @"-?[0-9\.]+").Success)
                {
                    if (!TryParseDouble(curToken, out var _))
                        return ResultDto<List<Token>>.Error(MathErrorMessager.NotNumberMessage(curToken));
                    if (resultTokens.Count > 0 && resultTokens[^1].Type == TokenType.Number)
                        return ResultDto<List<Token>>.Error(MathErrorMessager.TwoNumberInRowMessage(resultTokens[^1].Value, curToken));
                    tokenType = TokenType.Number;
                }
                else
                    return ResultDto<List<Token>>.Error(MathErrorMessager.UnknownCharacterMessage(curToken[0]));

                resultTokens.Add(new Token { Type = tokenType, Value = curToken });
                prevIsNegative = false;
            }

            if (resultTokens.Count > 0 && resultTokens[0].Type == TokenType.Operation)
                return ResultDto<List<Token>>.Error(MathErrorMessager.StartingWithOperation);
            if (resultTokens.Count > 0 && resultTokens[^1].Type == TokenType.Operation)
                return ResultDto<List<Token>>.Error(MathErrorMessager.EndingWithOperation);
            if (!CheckParanthesis(resultTokens))
                return ResultDto<List<Token>>.Error(MathErrorMessager.IncorrectBracketsNumber);

            return ResultDto<List<Token>>.Ok(resultTokens);
        }

        private void SetLastOperation(Stack<Expression> expressionsStack, Stack<string> operationsStack)
        {
            var rightExpr = expressionsStack.Pop();
            var leftExpr = expressionsStack.Pop();
            var operation = operationsStack.Pop();
            var mergedExpr = operationInfo[operation].expression(leftExpr, rightExpr);
            expressionsStack.Push(mergedExpr);
        }

        private ResultDto<Expression> CreateExpression(List<Token> tokens)
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

            return ResultDto<Expression>.Ok(expressionsStack.Pop());
        }
    }
}
