using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    [ExcludeFromCodeCoverage]
    public ActionResult Calculate([FromServices] ICalculator calculator,
        [FromServices] IParser parser,
        string val1,
        string operation,
        string val2)
    {
        var parserResult = new CalculatorService().Calculate(calculator, parser, val1, operation, val2, out var result);

        return parserResult switch
        {
            ParseStatus.InvalidNumber => BadRequest(Messages.InvalidNumberMessage),
            ParseStatus.InvalidOperation => BadRequest(Messages.InvalidOperationMessage),
            ParseStatus.DivisionByZero => Ok(Messages.DivisionByZeroMessage),
            ParseStatus.Success => Ok(result.ToString(CultureInfo.InvariantCulture))
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