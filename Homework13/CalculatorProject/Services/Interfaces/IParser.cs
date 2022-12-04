using CalculatorProject.Dto;

namespace CalculatorProject.Services.Interfaces
{
    public interface IParser
    {
        public ParserResult ParseExpression(string? expression);
    }
}
