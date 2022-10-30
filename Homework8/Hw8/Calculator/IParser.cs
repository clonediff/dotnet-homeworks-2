    namespace Hw8.Calculator
{
    public interface IParser
    {
        bool TryParseArgument(string value, out double result);

        bool TryParseOperation(string operation, out Operation result);
    }
}
