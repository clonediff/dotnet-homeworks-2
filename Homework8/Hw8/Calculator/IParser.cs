    namespace Hw8.Calculator
{
    public interface IParser
    {
        Result<(double val1, Operation oper, double val2), string> TryParseAllArguments(string arg1, string operation, string arg2);
    }
}
