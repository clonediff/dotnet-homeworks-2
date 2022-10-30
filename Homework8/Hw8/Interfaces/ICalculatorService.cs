using Hw8.Calculator;

namespace Hw8.Interfaces
{
    public interface ICalculatorService
    {
        Result<string> GetCalculationResult(string arg1, string operation, string aeg2);
    }
}
