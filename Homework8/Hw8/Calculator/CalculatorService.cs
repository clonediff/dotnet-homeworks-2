using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection.Metadata;

namespace Hw8.Calculator
{
    public class CalculatorService : ICalculatorService
    {
        private readonly ICalculator _calculator;
        private readonly IParser _parser;

        public CalculatorService(ICalculator calculator, 
            IParser parser)
        {
            _calculator = calculator;
            _parser = parser;
        }

        public Result<string> GetCalculationResult(string arg1,
            string operation,
            string arg2)
        {
            if (!_parser.TryParseArgument(arg1, out var val1)
            || !_parser.TryParseArgument(arg2, out var val2))
                return Result<string>.Error(Messages.InvalidNumberMessage);

            if (!_parser.TryParseOperation(operation, out var oper))
                return Result<string>.Error(Messages.InvalidOperationMessage);

            if (val2 == 0 && oper == Operation.Divide)
                return Result<string>.Error(Messages.DivisionByZeroMessage);


            var result = _calculator.Calculate(val1, oper, val2);
            return Result<string>.Ok(result.ToString(CultureInfo.InvariantCulture));
        }
    }
}
