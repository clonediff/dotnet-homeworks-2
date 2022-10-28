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

    public ParseStatus TryParseAllArguments(string arg1, string operation, string arg2, 
        out (double val1, Operation oper, double val2) result)
    { 
        result = default;

        if (!TryParseArgument(arg1, out var val1)
            || !TryParseArgument(arg2, out var val2))
            return ParseStatus.InvalidNumber;

        if (!TryParseOperation(operation, out var oper))
            return ParseStatus.InvalidOperation;

        if (val2 == 0 && oper == Operation.Divide)
            return ParseStatus.DivisionByZero;

        result = (val1, oper, val2);
        return ParseStatus.Success;
    }
}
