using Hw11.ErrorMessages;
using Hw11.Exceptions;
using System.Text.RegularExpressions;

namespace Hw11.Services.MathCalculator
{
    public class Tokenizer : ITokenizer
    {
        const string tokenizerRegex = @"\s*(\(\s*\-\s*[0-9\.]+\s*\)|[0-9\.]+|\S)\s*";

        private bool IsNumber(string token)
            => Regex.IsMatch(token, @"-?[0-9\.]+");
        private bool IsParanthesedNumber(string token)
            => Regex.IsMatch(token, @"\(-?[0-9\.]+\)");
        private bool IsOperation(string token)
            => Regex.IsMatch(token, @"[+\-*/]");
        private bool IsParanthesis(string token)
            => Regex.IsMatch(token, @"[\(\)]");

        public List<Token> GetTokens(string expression)
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
                if (prevIsNegative && IsNumber(curToken))
                    curToken = "-" + curToken;
                else if (prevIsNegative && !IsNumber(curToken))
                    resultTokens.Add(new Token { Type = TokenType.Operation, Value = "-" });
                else if (IsParanthesedNumber(curToken))
                    curToken = curToken.Substring(1, curToken.Length - 2);

                TokenType tokenType = default;
                if (IsParanthesis(curToken))
                    tokenType = TokenType.Paranthesis;
                else if (IsNumber(curToken))
                    tokenType = TokenType.Number;
                else if (IsOperation(curToken))
                    tokenType = TokenType.Operation;
                else
                    throw new InvalidSymbolException(MathErrorMessager.UnknownCharacterMessage(curToken[0]));

                resultTokens.Add(new Token { Type = tokenType, Value = curToken });
                prevIsNegative = false;
            }

            return resultTokens;
        }
    }
}
