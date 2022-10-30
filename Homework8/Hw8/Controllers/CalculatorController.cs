using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    [ExcludeFromCodeCoverage]
    public ActionResult Calculate(
        [FromServices] ICalculatorService calculatorService,
        string val1,
        string operation,
        string val2)
    {
        var result = calculatorService.GetCalculationResult(val1, operation, val2);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.ErrorValue);
    }

    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}