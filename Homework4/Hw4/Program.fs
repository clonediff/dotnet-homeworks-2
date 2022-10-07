open System.Diagnostics.CodeAnalysis
open Hw4.Parser
open Hw4.Calculator

[<ExcludeFromCodeCoverage>]
[<EntryPoint>]
let Main args =
    let parsedArgs = parseCalcArguments args
    let result = calculate parsedArgs.arg1 parsedArgs.operation parsedArgs.arg2
    printfn $"{result}"
    0