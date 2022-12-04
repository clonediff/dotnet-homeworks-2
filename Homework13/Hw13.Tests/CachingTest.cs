using CalculatorProject;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Hw13.Tests
{
    public class CachingTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CachingTest(WebApplicationFactory<Program> fixture)
        {
            _client = fixture.CreateClient();
        }


        [Theory]
        [InlineData("1 + 1 + 1 + 1")]
        [InlineData("2 * (3 + 2) / 2")]
        [InlineData("2 * 3 / 1 * 5 * 6")]
        public async Task Calculate_CacheTestAsync(string expression)
        {
            var firstCalculationTime = await GetRequestExecutionTime(expression);
            var secondCalculationTime = await GetRequestExecutionTime(expression);
            Assert.True(secondCalculationTime <= firstCalculationTime);
        }

        private async Task<long> GetRequestExecutionTime(string expression)
        {
            var watch = Stopwatch.StartNew();
            var response = await _client.PostCalculateExpressionAsync(expression);
            watch.Stop();
            response.EnsureSuccessStatusCode();
            return watch.ElapsedMilliseconds;
        }
    }
}
