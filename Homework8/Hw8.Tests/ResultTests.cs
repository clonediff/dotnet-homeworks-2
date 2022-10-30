using Hw8.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hw8.Tests;

public class ResultTests
{
    int OkValue = 42;
    string ErrorValue = "Error Value";

    [Fact]
    public void Ok_OkValue_ReturnsSuccessAndCorrectValue()
    {
        //act
        var result = Result<int>.Ok(OkValue);

        //assert
        Assert.True(result.IsSuccess);
        Assert.Equal(OkValue, result.Value);
    }

    [Fact]
    public void Error_ErrorValue_ReturnsSuccess()
    {
        //act
        var result = Result<int>.Error(ErrorValue);

        //assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorValue, result.ErrorText);
    }
}
