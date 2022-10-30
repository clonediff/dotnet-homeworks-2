namespace Hw8.Calculator
{
    public class Result<T, TError>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T Value { get; }
        public TError ErrorValue { get; }

        private Result(T value, bool success, TError error)
        {
            Value = value;
            IsSuccess = success;
            ErrorValue = error;
        }

        public static Result<T, TError> Ok(T value)
        {
            return new Result<T, TError>(value, true, default(TError));
        }

        public static Result<T, TError> Error(TError error)
        {
            return new Result<T, TError>(default(T), false, error);
        }
    }
}
