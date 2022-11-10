using System.Net.Http.Json;
using System.Text.Json;

namespace Hw9.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var expr = Console.ReadLine()!;
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("", expr)
            });
            var client = new HttpClient();
            try
            {
                var response = await client.PostAsync(@"https://localhost:7190/Calculator/CalculateMathExpression", content);
                var responseObjectJson = await response.Content.ReadAsStringAsync();
                var result = (JsonElement)JsonSerializer.Deserialize<object>(responseObjectJson)!;
                if (result.GetProperty("isSuccess").GetBoolean())
                    Console.WriteLine($"Answer: {result.GetProperty("result").GetRawText()}");
                else
                    Console.WriteLine($"Error: {result.GetProperty("errorMessage").GetRawText()}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка на сервере: {e.Message}");
            }
        }
    }
}