using Hw8.Calculator;

namespace Hw8.Interfaces;

public interface ICalculator
{
    double Calculate(double val1, Operation oper, double val2);

    double Plus(double val1, double val2);

    double Minus(double val1, double val2);

    double Multiply(double val1, double val2);

    double Divide(double firstValue, double secondValue);
}