module Hw6.App

open System
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open System.Net.Http
open Giraffe
open Hw5

let getMessageText (args:string[]) message =
    match message with
    | Message.WrongArgFormat -> match Parser.tryParseFloat args[0] with
                                | Ok _ -> $"Could not parse value '{args[2]}'"
                                | Error _ -> $"Could not parse value '{args[0]}'"
    | Message.WrongArgFormatOperation -> $"Could not parse value '{args[1]}'"
    | Message.DivideByZero -> "DivideByZero"

let getResultText args maybeResult=
    match maybeResult with 
    | Ok result -> result |> string |> Ok
    | Error msg -> match msg with
                    | Message.DivideByZero -> msg |> getMessageText args |> Ok
                    | _ -> msg |> getMessageText args |> Error

let calculate args = 
    let maybeParsedArgs = Parser.parseCalcArguments args
    match maybeParsedArgs with
    | Ok parsedArgs -> parsedArgs |||> Calculator.calculate |> Ok
    | Error message -> Error message

let convertOperation oper =
    match oper with
    | "Plus" -> "+"
    | "Minus" -> "-"
    | "Multiply" -> "*"
    | "Divide" -> "/"
    | _ -> oper

let calculatorHandler: HttpHandler =
    fun next ctx ->
        let query = ctx.Request.Query
        let args = [|
            query["value1"] |> string;
            query["operation"] |> string |> convertOperation;
            query["value2"] |> string
        |]

        let result: Result<string, string> = args |> calculate |> getResultText args

        match result with
        | Ok ok -> (setStatusCode 200 >=> text (ok.ToString())) next ctx
        | Error error -> (setStatusCode 400 >=> text error) next ctx

let webApp =
    choose [
        GET >=> choose [
             route "/" >=> text "Use /calculate?value1=<VAL1>&operation=<OPERATION>&value2=<VAL2>"
             route "/calculate" >=> calculatorHandler
        ]
        setStatusCode 404 >=> text "Not Found" 
    ]
    
type Startup() =
    member _.ConfigureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore

    member _.Configure (app : IApplicationBuilder) (_ : IHostEnvironment) (_ : ILoggerFactory) =
        app.UseGiraffe webApp


[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun whBuilder -> whBuilder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run()
    0