namespace Hw8.Calculator
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T Value { get; }
        public string ErrorText { get; }

        private Result(T value, bool success, string error)
        {
            Value = value;
            IsSuccess = success;
            ErrorText = error;
        }

        public static Result<T> Ok(T value)
        {
            return new Result<T>(value, true, default);
        }

        public static Result<T> Error(string error)
        {
            return new Result<T>(default(T), false, error);
        }
    }
}
