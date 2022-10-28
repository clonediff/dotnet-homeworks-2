    namespace Hw8.Calculator
{
    public interface IParser
    {
        ParseStatus TryParseAllArguments(string arg1, string operation, string arg2,
            out (double val1, Operation oper, double val2) result);
    }
}
