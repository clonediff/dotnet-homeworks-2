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
        
    let val1 = try float args[0] with _ -> ArgumentException() |> raise
    let val2 = try float args[2] with _ -> ArgumentException() |> raise
    let oper = parseOperation args[1]
    
    if (oper = CalculatorOperation.Undefined) then
        ArgumentException() |> raise
    
    {arg1 = val1; operation = oper; arg2 = val2}
