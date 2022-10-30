using System.Globalization;
using Hw8.Interfaces;

namespace Hw8.Calculator;

public class Parser : IParser
{
    public bool TryParseArgument(string value, out double result)
    {
        return double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result);
    }

    public bool TryParseOperation(string operation, out Operation result)
    {
        var parsed = Enum.TryParse(operation, true, out result);
        return result == Operation.Invalid ? false : parsed;
    }
}
