using Hw8.Calculator;

namespace Hw8.Interfaces
{
    public interface IParser
    {
        bool TryParseArgument(string value, out double result);

        bool TryParseOperation(string operation, out Operation result);
    }
}
