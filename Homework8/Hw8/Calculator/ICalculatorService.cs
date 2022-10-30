namespace Hw8.Calculator
{
    public interface ICalculatorService
    {
        Result<string, string> GetCalculationResult(string val1, string operation, string val2);
    }
}
