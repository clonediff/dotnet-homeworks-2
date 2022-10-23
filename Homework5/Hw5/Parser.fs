module Hw5.Parser

open System
open System.Globalization
open System.Diagnostics.CodeAnalysis
open Hw5.Calculator
open Hw5.MaybeBuilder

let isArgLengthSupported (args:string[]): Result<'a,'b> =
    match args.Length with
    | 3 -> Ok args
    | _ -> Error Message.WrongArgLength
 
[<ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | Calculator.plus -> Ok (arg1, CalculatorOperation.Plus, arg2)
    | Calculator.minus -> Ok (arg1, CalculatorOperation.Minus, arg2)
    | Calculator.multiply -> Ok (arg1, CalculatorOperation.Multiply, arg2)
    | Calculator.divide -> Ok (arg1, CalculatorOperation.Divide, arg2)
    | _ -> Error Message.WrongArgFormatOperation

[<ExcludeFromCodeCoverage>]
let tryParseFloat(str: string): Result<float, Message> =
    match Double.TryParse(str, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture) with
    | (true, value) -> Ok value
    | _ -> Error Message.WrongArgFormat

let parseArgs (args: string[]): Result<('a * string * 'b), Message> =
    match tryParseFloat args[0] with
    | Ok arg1 -> match tryParseFloat args[2] with
                    | Ok arg2 -> Ok (arg1, args[1], arg2)
                    | Error e -> Error e
    | Error e -> Error e

[<ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation = CalculatorOperation.Divide && arg2 = 0.0 with
    | true -> Error Message.DivideByZero
    | _ -> Ok (arg1, operation, arg2)

let parseCalcArguments (args: string[]): Result<'a, 'b> =
    maybe {
        let! checkedArgs = isArgLengthSupported args
        let! parsedArgs = parseArgs checkedArgs
        let! operationParse = isOperationSupported parsedArgs
        let! isDividingByZero = isDividingByZero operationParse
        return isDividingByZero
    }