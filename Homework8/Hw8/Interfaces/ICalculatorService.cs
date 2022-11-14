using Hw8.Calculator;

namespace Hw8.Interfaces
{
    public interface ICalculatorService
    {
        Result<double> GetCalculationResult(string arg1, string operation, string arg2);
    }
}
