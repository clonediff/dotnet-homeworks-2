using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator
{
    public interface ICalculator
    {
        public Task<double> CalculateAsync(Expression expression);
    }
}
