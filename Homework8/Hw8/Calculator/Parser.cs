using System.Globalization;

namespace Hw8.Calculator;

public class Parser
{
    public static double ParseArgument(string value)
    {
        if (!double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
            result = double.NaN;
        return result;
    }

    public static Operation ParseOperation(string operation)
    {
        if (!Enum.TryParse<Operation>(operation, true, out var result))
            result = Operation.Invalid;
        return result;
    }
}
