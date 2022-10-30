namespace Hw8.Calculator
{
    public interface ICalculatorService
    {
        Result<string> GetCalculationResult(string arg1, string operation, string aeg2);
    }
}
