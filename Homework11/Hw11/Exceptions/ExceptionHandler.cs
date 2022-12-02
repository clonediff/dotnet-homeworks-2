using System.Diagnostics.CodeAnalysis;

namespace Hw11.Exceptions;

public class ExceptionHandler : IExceptionHandler
{
	public const string UnknownError = "Unknown error";
	public const string InvalidNumber = "Invalid number";
	public const string InvalidSyntax = "Invalid syntax";
	public const string InvalidSymbol = "Invalid symbol";
	
	private readonly ILogger<ExceptionHandler> _logger;

	public ExceptionHandler(ILogger<ExceptionHandler> logger)
	{
		_logger = logger;
	}

	public void HandleException<T>(T exception) where T : Exception
	{
		this.Handle((dynamic) exception);
	}
	
	[ExcludeFromCodeCoverage]
	private void Handle(Exception exception)
	{
		_logger.LogError($"{UnknownError}: {exception.Message}");
	}

	private void Handle(InvalidNumberException exception)
	{
		_logger.LogError($"{InvalidNumber}: {exception.Message}");
	}

	private void Handle(InvalidSyntaxException exception)
	{
		_logger.LogError($"{InvalidSyntax}: {exception.Message}");
	}

	private void Handle(InvalidSymbolException exception)
	{
		_logger.LogError($"{InvalidSymbol}: {exception.Message}");
	}
	
	private void Handle(DivideByZeroException exception)
	{
		_logger.LogError(exception.Message);
	}
}