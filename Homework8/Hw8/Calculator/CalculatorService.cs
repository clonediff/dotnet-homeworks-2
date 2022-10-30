using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection.Metadata;

namespace Hw8.Calculator
{
    public class CalculatorService : ICalculatorService
    {
        private ICalculator _calculator;
        private IParser _parser;

        public CalculatorService([FromServices]ICalculator calculator, 
            [FromServices]IParser parser)
        {
            _calculator = calculator;
            _parser = parser;
        }

        private Result<double, string> Calculate(string val1,
            string operation,
            string val2)
        {

            var maybeParsed = _parser.TryParseAllArguments(val1, operation, val2);

            if (maybeParsed.IsSuccess)
            {
                var arg1 = maybeParsed.Value.val1;
                var oper = maybeParsed.Value.oper;
                var arg2 = maybeParsed.Value.val2;
                return Result<double, string>.Ok(_calculator.Calculate(arg1, oper, arg2));
            }

            return Result<double, string>.Error(maybeParsed.ErrorValue);
        }

        public Result<string, string> GetCalculationResult(string val1,
            string operation,
            string val2)
        {
            var result = Calculate(val1, operation, val2);
            if (result.IsSuccess)
                return Result<string, string>.Ok(result.Value.ToString(CultureInfo.InvariantCulture));

            return Result<string, string>.Error(result.ErrorValue);
        }
    }
}
