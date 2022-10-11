open System
open Hw5

let getMessageText message =
    match message with
    | Message.WrongArgLength -> "Количество аргументов должно быть равно 3"
    | Message.WrongArgFormatOperation -> "Калькулятор может выполнять только 4 операции: + - * /"
    | Message.WrongArgFormat -> "Калькулятор может использовать только числа"
    | Message.DivideByZero -> "Нельзя делить на ноль"

let calc (val1, operation, val2) =
    Calculator.calculate val1 operation val2

[<EntryPoint>]
let Main args =
    let maybeParsedArgs = Parser.parseCalcArguments args
    match maybeParsedArgs with
    | Ok parsedArgs -> printfn $"{calc parsedArgs}"
    | Error message -> printfn $"{getMessageText message}"
    0