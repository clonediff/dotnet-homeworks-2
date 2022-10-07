module Hw4.Parser

open System
open Hw4.Calculator
open Microsoft.FSharp.Core


type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let isArgLengthSupported (args : string[]) =
    args.Length = 3

let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> CalculatorOperation.Undefined

let parseCalcArguments(args : string[]) = 
    if (isArgLengthSupported(args) |> not) then
        ArgumentException() |> raise
    
    let couldParse1, val1 = Double.TryParse args[0]
    let couldParse2, val2 = Double.TryParse args[2]
    let oper = parseOperation args[1]
    
    if (oper = CalculatorOperation.Undefined ||
        not couldParse1 ||
        not couldParse2) then
        ArgumentException() |> raise
    
    {arg1 = val1; operation = oper; arg2 = val2}
