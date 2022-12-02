using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator
{
    public interface IParser
    {
        public Expression ParseExpression(string? expression);
    }
}
