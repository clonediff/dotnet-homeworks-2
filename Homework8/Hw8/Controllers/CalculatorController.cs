using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<string> Calculate([FromServices] ICalculator calculator,
        string val1,
        string operation,
        string val2)
    {
        var arg1 = Parser.ParseArgument(val1);
        var arg2 = Parser.ParseArgument(val2);
        var oper = Parser.ParseOperation(operation);

        if (double.IsNaN(arg1) || double.IsNaN(arg2))
            return Messages.InvalidNumberMessage;
        if (oper == Operation.Invalid)
            return Messages.InvalidOperationMessage;
        if (arg2 == 0 && oper == Operation.Divide)
            return Messages.DivisionByZeroMessage;

        return Calculate(calculator, arg1, oper, arg2).ToString(CultureInfo.InvariantCulture);
    }

    [ExcludeFromCodeCoverage]
    private double Calculate(ICalculator calculator, double arg1, Operation operation, double arg2)
    {
        return operation switch
        {
            Operation.Plus => calculator.Plus(arg1, arg2),
            Operation.Minus => calculator.Minus(arg1, arg2),
            Operation.Multiply => calculator.Multiply(arg1, arg2),
            Operation.Divide => calculator.Divide(arg1, arg2)
        };
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}