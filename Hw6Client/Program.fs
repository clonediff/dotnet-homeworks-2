open System
open System.Net.Http
open System.Threading.Tasks

let transfromLineToArgs (input: string) = 
    let args = input.Split(' ')
    match args.Length with
    | 0 -> [|" "; " "; " "|]
    | 1 -> [|args[0]; " "; " "|]
    | 2 -> [|args[0]; args[1]; " "|]
    | _ -> args

let getResponse (uri: Uri) (client: HttpClient) = 
    async{
        try
            let! response = client.GetStringAsync(uri) |> Async.AwaitTask
            // Имитация длительного процесса на сервере
            let! delay = Task.Delay(500) |> Async.AwaitTask
            return response
        with
        | :? AggregateException as e -> return "Bad request. Can't calculate with this arguments"
    }

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