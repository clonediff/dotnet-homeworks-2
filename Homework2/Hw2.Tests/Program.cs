using Hw2;

double val1, val2;
CalculatorOperation operation;

Parser.ParseCalcArguments(args, out val1, out operation, out val2);
var result = Calculator.Calculate(val1, operation, val2);
Console.WriteLine(result);