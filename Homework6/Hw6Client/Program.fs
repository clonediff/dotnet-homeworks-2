open System
open System.Net.Http
open System.Threading.Tasks

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let transfromLineToArgs (input: string) = 
    let args = input.Split(' ')
    match args.Length with
    | 0 -> [|" "; " "; " "|]
    | 1 -> [|args[0]; " "; " "|]
    | 2 -> [|args[0]; args[1]; " "|]
    | _ -> args

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let parseResponse (response: HttpResponseMessage) = 
    let responseMsg = response.Content.ReadAsStringAsync() 
                        |> Async.AwaitTask 
                        |> Async.RunSynchronously
    match response.IsSuccessStatusCode with
    | true -> $"Answer: {responseMsg}"
    | _ -> $"Bad request: {responseMsg}"
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let getResponse (uri: Uri) (client: HttpClient) = 
    async{
        let! response = client.GetAsync(uri) |> Async.AwaitTask
        let result = response |> parseResponse 
        // Имитация длительного процесса на сервере
        let! delay = Task.Delay(500) |> Async.AwaitTask
        return result
    }
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
[<EntryPoint>]
let main _ =
    let webClient = new HttpClient()
    printfn "Enter port for localhost"
    let port = Console.ReadLine()
    printfn "Print <value1> <operation> <value2>"
    while(true) do
        let input = Console.ReadLine() |> transfromLineToArgs
        let uri = new Uri($"https://localhost:{port}/calculate?value1={input[0]}&operation={input[1]}&value2={input[2]}")
        printfn $"{webClient |> getResponse uri |> Async.RunSynchronously}\n"
    0