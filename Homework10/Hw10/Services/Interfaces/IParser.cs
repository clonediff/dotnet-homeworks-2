using Hw10.Dto;

namespace Hw10.Services.Interfaces
{
    public interface IParser
    {
        public ParserResult ParseExpression(string? expression);
    }
}
