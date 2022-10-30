using Microsoft.AspNetCore.Mvc;

namespace Hw8.Calculator
{
    public class CalculatorService
    {
        public ParseStatus Calculate([FromServices] ICalculator calculator,
        [FromServices] IParser parser,
        string val1,
        string operation,
        string val2, out double result)
        {
            var parserResult = parser.TryParseAllArguments(val1, operation, val2, out var parsed);

            result = default;
            if (parserResult == ParseStatus.Success)
                result = calculator.Calculate(parsed.val1, parsed.oper, parsed.val2);

            return parserResult;
        }
    }
}
