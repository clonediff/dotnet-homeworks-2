using System.Globalization;

namespace Hw8.Calculator;

public class Parser : IParser
{
    private bool TryParseArgument(string value, out double result)
    {
        return double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result);
    }

    private bool TryParseOperation(string operation, out Operation result)
    {
        var parsed = Enum.TryParse(operation, true, out result);
        return result == Operation.Invalid ? false : parsed;
    }

    public Result<(double val1, Operation oper, double val2), string> TryParseAllArguments(
        string arg1, 
        string operation, 
        string arg2)
    { 
        if (!TryParseArgument(arg1, out var val1)
            || !TryParseArgument(arg2, out var val2))
            return Result<(double val1, Operation oper, double val2), string>.Error(Messages.InvalidNumberMessage);

        if (!TryParseOperation(operation, out var oper))
            return Result<(double val1, Operation oper, double val2), string>.Error(Messages.InvalidOperationMessage);

        if (val2 == 0 && oper == Operation.Divide)
            return Result<(double val1, Operation oper, double val2), string>.Error(Messages.DivisionByZeroMessage);

        var result = (val1, oper, val2);
        return Result<(double val1, Operation oper, double val2), string>.Ok(result);
    }
}
