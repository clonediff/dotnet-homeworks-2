using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult Calculate([FromServices] ICalculator calculator,
        [FromServices] IParser parser,
        string val1,
        string operation,
        string val2)
    {
        var parserResult = parser.TryParseAllArguments(val1, operation, val2, out var parsed);

        return parserResult switch
        {
            ParseStatus.InvalidNumber => BadRequest(Messages.InvalidNumberMessage),
            ParseStatus.InvalidOperation => BadRequest(Messages.InvalidOperationMessage),
            ParseStatus.DivisionByZero => Ok(Messages.DivisionByZeroMessage),
            ParseStatus.Success => Ok(calculator.Calculate(parsed.val1, parsed.oper, parsed.val2).ToString(CultureInfo.InvariantCulture))
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