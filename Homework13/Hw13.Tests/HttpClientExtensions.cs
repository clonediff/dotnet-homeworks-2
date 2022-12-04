﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hw13.Tests;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> PostCalculateExpressionAsync(this HttpClient client, string expression)
    {
        var stringContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"expression", expression}
        });

        var response = await client.PostAsync("/Calculator/CalculateMathExpression", stringContent);
        return response;
    }
}